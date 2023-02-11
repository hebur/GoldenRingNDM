import pandas as pd

ct = pd.read_csv("../../StreamingAssets/carddata.csv")

file_path = "../../StreamingAssets/cards_.json"

cards = []

for i in range(len(ct)):
  id = ct.Level[i] * 100 + ct.No[i]
  price = [ct.AirP[i], ct.FireP[i], ct.EarthP[i], ct.WaterP[i]]
  effect = [ct.AirE[i], ct.FireE[i], ct.EarthE[i], ct.WaterE[i]]
  turn = ct.Turn[i]
  slot = ct.Slot[i]
  score = ct.Score[i]
  cards.append({
    "id": id,
    "price": [0] + price + [0, 0, 0, 0, 0],
    "effect": [0] + effect + [0, 0],
    "turn": turn,
    "slot": slot,
    "score": score
  })

with open(file_path, 'w') as outfile:
  outfile.write("{\n")
  outfile.write("  \"cards\": [\n")
  leng = len(cards);
  i = 0
  for card in cards:
    i = i + 1
    outfile.write("    {\n")
    outfile.write("      \"id\": " + str(card["id"]) + ",\n")
    outfile.write("      \"price\": " + str(card["price"]) + ",\n")
    outfile.write("      \"effect\": " + str(card["effect"]) + ",\n")
    outfile.write("      \"turn\": " + str(card["turn"]) + ",\n")
    outfile.write("      \"slot\": " + str(card["slot"]) + ",\n")
    outfile.write("      \"score\": " + str(card["score"]) + "\n")
    outfile.write("    }")
    if i != leng:
      outfile.write(",\n")
    else:
      outfile.write("\n")
  outfile.write("  ]\n")
  outfile.write("}")

