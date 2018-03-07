import json
from collections import defaultdict
from urllib.request import urlretrieve
import os
from tqdm import tqdm
import pyimgur, webbrowser
from PIL import Image
import requests

CLIENT_ID = "042828e8b214276"
CLIENT_SECRET = "ca89d8a57ae848226b06f545bc0198b94878f5cb"
CLIENT_TOKEN, CLIENT_REFRESH = None, None
CLIENT_TOKEN, CLIENT_REFRESH = "88c35fd11ca06e8362cd49381f5d3cfc5658ee22", "cc323c7a202b84b351ac78a324c1ddd106f1683c"

class RelicDataEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, defaultdict):
            return dict(obj)
        elif isinstance(obj, set):
            return list(obj)
        return super(RelicDataEncoder, self).default(obj)

with open("warframe-drop-data/data/relicsCompiled.json") as inf:
    j=json.load(inf)

with open("node_modules/warframe-item-data/data/itemThumbs.json") as inf:
    thumbs=dict((d["name"].lower().replace("<archwing> ",""), d) for d in json.load(inf))

images = set()
suffixes = ["Band","Barrel","Blades","Blade","Boot","Buckle","Carapace","Cerebrum","Chassis","Chassis Blueprint","Disc","Gauntlet","Grip","Guard","Handle","Harness Blueprint","Head","Hilt","Kubrow Collar Blueprint","Link","Lower Limb","Neuroptics","Neuroptics Blueprint","Ornament","Pouch","Receiver","Stars","Stock","String","Systems","Systems Blueprint","Upper Limb","Wings Blueprint","Blueprint"]
itemNames = list(set(item['itemName'] for relic in j for rewardsByTier in relic['rewards'].values() for item in rewardsByTier))

itemPrefixes = set()
for item in itemNames:
    broke=False
    for suffix in suffixes:
        if item.endswith(suffix):
            prefix = item[:-len(suffix)-1]
            itemPrefixes.add(prefix)
            broke=True
            break
    if not broke:
        print(item)

itemPrefixes=[i.lower() for i in itemPrefixes]

relicDrops=defaultdict(lambda :defaultdict(set))
for relic in j:
    relicName = relic['name']
    for rewards in relic['rewards'].values():
        for reward in rewards:
            itemPrefix = None
            itemName = reward['itemName'].lower()
            imageName = thumbs[itemName.lower().replace(" blueprint","")]["textureLocation"]
            images.add(imageName)
            for prefix in itemPrefixes:
                if itemName.startswith(prefix):
                    itemPrefix = prefix
                    break
            if itemPrefix is None:
                print(reward['itemName'])
                continue
            relicDrops[itemPrefix][itemName[len(itemPrefix)+1:]].add((relicName, reward['rarity'], relic['tier'], imageName.split("/")[-1][:-4]))

if os.path.exists("imgur.json"):
    with open("imgur.json") as inf:
        imgur_credentials = json.load(inf)
        imgur = pyimgur.Imgur(CLIENT_ID, CLIENT_SECRET, imgur_credentials["access_token"], imgur_credentials["refresh_token"])
elif CLIENT_TOKEN is not None and CLIENT_REFRESH is not None:
    imgur = pyimgur.Imgur(CLIENT_ID, CLIENT_SECRET, CLIENT_TOKEN, CLIENT_REFRESH)
else:
    imgur = pyimgur.Imgur(CLIENT_ID, CLIENT_SECRET)
    auth_url = imgur.authorization_url('pin')
    print(auth_url)
    pin = input("What is the pin? ") # Python 3x
    imgur.exchange_pin(pin)
    with open("imgur.json","w") as outf:
        json.dump({"access_token":imgur.access_token, "refresh_token":imgur.refresh_token}, outf)

album = imgur.get_album("AZsd0")

if os.path.exists("../data/image_urls.json"):
    with open("../data/image_urls.json") as inf:
        image_urls = json.load(inf)
else:
    image_urls = {}

for image in tqdm(images):
    fn = image.split("/")[-1]
    path = "../images/items/original/{}".format(fn)
    out_path = "../images/items/out/{}".format(fn)
    if not os.path.exists(path):
        res = urlretrieve("http://content.warframe.com/MobileExport{}".format(image), path)
    if not os.path.exists(out_path):
        img = Image.open(path)
        img.resize((512,342))
        img.save(out_path)
    title = image.split("/")[-1][:-4]
    if title not in image_urls:
        try:
            uploaded_image = imgur.upload_image(out_path, title=title, album=album)
        except requests.exceptions.HTTPError as e:
            print(e.response.headers)
            print(e.request.headers)
        image_urls[title] = uploaded_image.link
        with open("../data/image_urls.json","w") as outf:
            json.dump(image_urls, outf)

rarityOrder = {"Common":0, "Uncommon":1, "Rare":2}
formattedDrops = {'images':images, 'prefixes':itemPrefixes, 'suffixes':sorted([i.lower() for i in suffixes]), 'items':[]}
for relicPrefix, suffixes in relicDrops.items():
    prefixNode={"itemPrefix":relicPrefix, "itemSuffixes":[]}
    for relicSuffix, drops in suffixes.items():
        prefixNode['itemSuffixes'].append({"suffix":relicSuffix, "image":image_urls[list(drops)[0][3]], "relics":sorted([{"name": drop[0], "rarity":drop[1], "era":drop[2], "image":image_urls[drop[3]]} for drop in drops], key=lambda x:rarityOrder[x["rarity"]])})
    formattedDrops['items'].append(prefixNode)

with open("../data/relicDropData.json","w") as outf:
    json.dump(formattedDrops, outf, cls=RelicDataEncoder)