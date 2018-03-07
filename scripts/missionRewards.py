import json, re
from collections import defaultdict

with open("warframe-drop-data/data/missionRewards.json") as inf:
    j=json.load(inf)

allRewards = defaultdict(list)

multipleItemTest = re.compile("^(\d+)X? (.*?)$")
batchItemTest = re.compile("^(\d+)X? (\d+)X? (.*?)$")
indexTest = re.compile("^Return: ([\d,]+)$")

def add_reward(base_entry, item_name, chance, rotation=None):
    entry = base_entry.copy()
    entry.update({"chance": chance})
    if rotation:
        entry.update({"rotation": rotation})
    multiple_match = multipleItemTest.match(item_name)
    batch_match = batchItemTest.match(item_name)
    index_match = indexTest.match(item_name)
    if index_match:
        entry.update({"amount": int(index_match[1].replace(",",""))})
        allRewards["Credits"].append(entry)
    elif batch_match:
        amount1, amount2, actual_item_name = int(batch_match[1]), int(batch_match[2]), batch_match[3]
        entry.update({"amount": amount1 * amount2})
        allRewards[actual_item_name].append(entry)
    elif multiple_match:
        amount, actual_item_name = int(multiple_match[1]), multiple_match[2]
        entry.update({"amount": amount})
        allRewards[actual_item_name].append(entry)
    else:
        allRewards[item_name].append(entry)

for planetName, nodes in j['missionRewards'].items():
    for nodeName, nodeInfo in nodes.items():
        if not nodeInfo["isEvent"]:
            baseEntry = {"planet": planetName, "node": nodeName, "mission": nodeInfo["gameMode"]}
            if type(nodeInfo["rewards"]) is dict:
                for rotation, rewards in nodeInfo["rewards"].items():
                    for reward in rewards:
                        add_reward(baseEntry, reward["itemName"], reward["chance"], rotation=rotation)
            elif type(nodeInfo["rewards"]) is list:
                for reward in rewards:
                    add_reward(baseEntry, reward["itemName"], reward["chance"])


outRewards = []
for rewardName, locations in allRewards.items():
    outRewards.append({"item":rewardName, "locations":sorted(locations, key=lambda x:x["chance"], reverse=True)})
with open("../data/rewardLocations.json","w") as outf:
    json.dump(outRewards, outf)
for key in sorted(list(allRewards.keys())):
    if key == "Credits":
        for reward in allRewards[key]:
            print(reward)