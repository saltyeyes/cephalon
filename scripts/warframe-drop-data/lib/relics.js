const {hash, parseRelic, parseChance} = require("./utils.js")

const rarities = {
    Intact: {
        25.33: 'Common',
        11: 'Uncommon',
        2: 'Rare'
    },
    Exceptional: {
        23.33: 'Common',
        13: 'Uncommon',
        4: 'Rare'
    },
    Flawless: {
        20: 'Common',
        17: 'Uncommon',
        6: 'Rare'
    },
    Radiant: {
        16.67: 'Common',
        20: 'Uncommon',
        10: 'Rare'
    }
}

module.exports = function($) {
    const table = $("#relicRewards").next("table")

    const tbody = table.children()['0']

    let relic = null

    let relics = []

    for(let tr of tbody.children) {
        let elem = tr.children[0]
        let text = $(elem).text()

        if(elem.name === "th") {

            if(relic) {
                relics.push(relic)
            }

            let tmp = parseRelic(text)

            if(tmp) {
                relic = tmp
                relic._id = hash(`${relic.tier}_${relic.relicName}_${relic.state}`)
            }
        }

        if(elem.name === "td" && elem.attribs.class !== "blank-row") {
            let chanceElem = tr.children[1]
            let chance = parseChance($(chanceElem).text())

            relic.rewards.push({
                _id: hash(text),
                itemName: text,
                rarity: rarities[relic.state][Number(chance.chance)],
                chance: Number(chance.chance)
            })
        }
    }

    // push the last one too
    relics.push(relic)

    return relics
}