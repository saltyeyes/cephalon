import requests, json, os, re
import lua
urls={
    "weapon": "http://warframe.wikia.com/wiki/Module:Weapons/data?action=edit",
    "warframe": "http://warframe.wikia.com/wiki/Module:Warframes/data?action=edit",
    "dropTables": "http://warframe.wikia.com/wiki/Module:DropTables/data?action=edit",
    "relics": "http://warframe.wikia.com/wiki/Module:Void/data?action=edit",
    "icons": "http://warframe.wikia.com/wiki/Module:Icon/data?action=edit",
    "modules": "http://warframe.wikia.com/wiki/Module:Mods/data?action=edit",
    "research": "http://warframe.wikia.com/wiki/Module:Research/data?action=edit",
    "focus": "http://warframe.wikia.com/wiki/Module:Focus/data?action=edit",
    "arcanes": "http://warframe.wikia.com/wiki/Module:Arcane/data?action=edit",
    "missions": "http://warframe.wikia.com/wiki/Module:Missions/data?action=edit",
    "abilities": "http://warframe.wikia.com/wiki/Module:Ability/data?action=edit"
}

for category, url in urls.items():
    if os.path.exists("./cache/{}.cache".format(category)):
        with open("./cache/{}.cache".format(category)) as inf:
            data = inf.read()
    else:
        data = requests.get(url).content.decode()
        with open("./cache/{}.cache".format(category),"w") as outf:
            outf.write(data)

    rawdata = data.split(" name=\"wpTextbox1\">")[1].split("</textarea>")[0]

    with open("json.lua") as inf:
        ljson = lua.execute(inf.read())
    d=lua.execute(rawdata)

    with open("../data/{}WikiData.json".format(category),"w") as outf:
        outf.write(ljson.encode(d))

semlar = requests.get("https://semlar.com/buffs")
for image_path in re.findall("<img src=\"(https://semlar.com/textures/.*?\.png)\""):
    if 