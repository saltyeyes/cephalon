import json, requests, os
from PIL import Image
from urllib.request import urlretrieve
from tqdm import tqdm

if os.path.exists("../data/enemyModDrops.json"):
    enemyModDrops = json.load(open("../data/enemyModDrops.json"))
else:
    all_enemies = dict((i['name'].upper(), i) for i in json.loads(requests.get("http://content.warframe.com/MobileExport/Manifest/ExportEnemies.json").content.replace(b"\\r",b"").replace(b"\n",b"\\n"))['ExportEnemies'])
    images = dict((i['uniqueName'], i) for i in requests.get("http://content.warframe.com/MobileExport/Manifest/ExportManifest.json").json()["Manifest"])

    j=json.load(open("warframe-drop-data/data/modLocations.json"))["modLocations"]

    out_array = []

    for mod in j:
        new_mod = {"name": mod["modName"], "enemies":[]}
        for enemy in mod["enemies"]:
            if enemy["enemyName"].upper() in all_enemies:
                new_enemy = {"name": enemy["enemyName"], "modDropChance": enemy["enemyModDropChance"], "chance": enemy["chance"]}
                new_enemy["image"] = images[all_enemies[enemy["enemyName"].upper()]["uniqueName"]]["textureLocation"].replace("\\","/")
                new_mod["enemies"].append(new_enemy)
            else:
                print(enemy["enemyName"])
        out_array.append(new_mod)

    with open("../data/enemyModDrops.json","w") as outf:
        json.dump(out_array, outf)

    enemyModDrops = out_array

all_images = list(set([i["image"] for k in enemyModDrops for i in k["enemies"]]))

if os.path.exist("cache/enemy-image-urls.cache"):
    enemy_urls = json.load(open("cache/enemy-image-urls.cache"))
else:
    enemy_urls = {}

os.makedirs("../images/enemies/original", exist_ok=True)
os.makedirs("../images/enemies/resized", exist_ok=True)

for image in tqdm(all_images):
    path = "../images/enemies/original"+image
    if not os.path.exists(path):
        dirs = path.rsplit("/", maxsplit=1)[0]
        os.makedirs(dirs, exist_ok=True)
        urlretrieve("http://content.warframe.com/MobileExport"+image, path)
    
    out_path = "../images/enemies/resized"+image
    if not os.path.exists(out_path):
        img=Image.open(path)
        img.resize((512,342))
        img.save(out_path)
    
    