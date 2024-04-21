using StringLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NutriLensClassLibrary.Models
{
    public static class TacoFoodItemParseHelperClass
    {
        private static string[] excludedWords = new[] { "de", "com", "sem", "em", "tipo", "inteiro", "à", "grande", "para", "da", "ao", "e", "do" };

        public static List<TacoItem> TacoFoodItems { get; set; }

        public static FoodItem Parse(SimpleFoodItem simpleFoodItem)
        {
            Dictionary<int, int> tacoMatchesScores = new Dictionary<int, int>();

            foreach (TacoItem tacoItem in TacoFoodItems)
            {
                string tacoItemName = tacoItem.Nome.ToUpper();

                if (tacoItemName.Contains("CRU"))
                {
                    if (tacoItemName.Contains("PORCO")
                        || tacoItemName.Contains("FRANGO")
                        || tacoItemName.Contains("PEIXE")
                        || tacoItemName.Contains("CARNE")
                        || tacoItemName.Contains("ARROZ")
                        || tacoItemName.Contains("FEIJÃO")
                        || tacoItemName.Contains("MACARRÃO")
                        || tacoItemName.Contains("OVO"))
                        continue;
                }

                if(simpleFoodItem.Item.ToUpper().Contains("BATATAS") && simpleFoodItem.Item.ToUpper().Contains("FRITAS"))
                {
                    simpleFoodItem.Item = simpleFoodItem.Item.Replace("BATATAS", "BATATA");
                    simpleFoodItem.Item = simpleFoodItem.Item.Replace("FRITAS", "FRITA");
                }

                if (Regex.IsMatch(tacoItem.Nome, @"LING[ÜU]IÇA", RegexOptions.IgnoreCase) && !Regex.IsMatch(simpleFoodItem.Item, @"LING[ÜU]IÇA", RegexOptions.IgnoreCase))
                    continue;

                if (Regex.IsMatch(tacoItem.Nome, @"CORAÇÃO", RegexOptions.IgnoreCase) && !Regex.IsMatch(simpleFoodItem.Item, @"CORAÇÃO", RegexOptions.IgnoreCase))
                    continue;

                if (!Regex.IsMatch(tacoItem.Nome, @"BATATA", RegexOptions.IgnoreCase) && Regex.IsMatch(simpleFoodItem.Item, @"BATATA", RegexOptions.IgnoreCase))
                    continue;

                //if (!Regex.IsMatch(tacoItem.Nome, @"BATATA.*FRITA") && Regex.IsMatch(simpleFoodItem.Item, @"BATATA.*FRITA", RegexOptions.IgnoreCase))
                //    continue;

                bool gotWordFullMatch = false;
                int totalSimilarity = 0;

                int similarity = StringFunctions.HerbertL_Algorithm.CompareEdited(tacoItem.Nome, simpleFoodItem.Item);

                if (similarity == 255)
                    gotWordFullMatch = true;

                if (similarity > 127)
                    totalSimilarity = similarity;


                if (simpleFoodItem.Item.ToUpper().Contains("ARROZ BRANCO"))
                    simpleFoodItem.Item = simpleFoodItem.Item.Replace("BRANCO", "TIPO 1");

                if (simpleFoodItem.Item.ToUpper() == "ARROZ")
                    simpleFoodItem.Item = simpleFoodItem.Item.Replace("ARROZ", "ARROZ TIPO 1");

                if (simpleFoodItem.Item.ToUpper().Contains("LINGUIÇA") && !simpleFoodItem.Item.ToUpper().Contains("PORCO"))
                    simpleFoodItem.Item = simpleFoodItem.Item.Replace("LINGUIÇA", "LINGUIÇA PORCO");

                foreach (string tacoItemWord in tacoItem.Nome.Split(' '))
                {
                    if (!ValidWordForAnalysis(tacoItemWord))
                        continue;

                    foreach (string simpleFoodItemWord in simpleFoodItem.Item.Split(' '))
                    {
                        if (!ValidWordForAnalysis(tacoItemWord))
                            continue;

                        similarity = StringFunctions.HerbertL_Algorithm.CompareEdited(tacoItemWord, simpleFoodItemWord);

                        if (similarity == 255)
                            gotWordFullMatch = true;
                        if (similarity > 127)
                            totalSimilarity += similarity;
                    }
                }

                if (gotWordFullMatch)
                    tacoMatchesScores.Add(tacoItem.id, totalSimilarity);
                else
                    tacoMatchesScores.Add(tacoItem.id, 0);
            }

            TacoItem tacoItemToAdd;

            // Find the key (Taco Item ID) with the highest score
            var highestScoreTacoItems = tacoMatchesScores
                .OrderByDescending(x => x.Value)
                .Take(10)
                .ToList();

            Console.WriteLine("Item: " + simpleFoodItem.Item + Environment.NewLine);


            foreach (var scoredTacoItem in highestScoreTacoItems)
            {
                Console.WriteLine(TacoFoodItems.Find(x => x.id == scoredTacoItem.Key) + " - " + scoredTacoItem.Value);
            }

            Console.WriteLine();

            tacoItemToAdd = TacoFoodItems.Find(x => x.id == highestScoreTacoItems[0].Key);

            string tacoNome = tacoItemToAdd.Nome;
            string gptPortion = simpleFoodItem.Quantity.ToString();
            double kcal = double.Parse(gptPortion);
            double kcalTbca = Convert.ToDouble(tacoItemToAdd.EnergiaKcal);
            double kiloCalories = (kcal * kcalTbca) / 100;

            return new FoodItem()
            {
                Name = tacoNome,
                Portion = gptPortion,
                KiloCalories = kiloCalories,
                TacoFoodItem = GetTacoFoodItem(tacoItemToAdd, gptPortion)
            };
        }

        private static bool ValidWordForAnalysis(string word)
        {
            if (excludedWords.Contains(word.ToLower()))
                return false;

            if (Regex.IsMatch(word, @"[0-9]+"))
                return false;

            return true;
        }

        private static TacoItem GetTacoFoodItem(TacoItem taco, string portion)
        {
            double portionDouble = double.Parse(portion);

            taco.Umidade = (taco.GetValue(nameof(taco.Umidade)) * portionDouble / 100);
            taco.Proteina = (taco.GetValue(nameof(taco.Proteina)) * portionDouble / 100);
            taco.Lipideos = (taco.GetValue(nameof(taco.Lipideos)) * portionDouble / 100);
            taco.Colesterol = (taco.GetValue(nameof(taco.Colesterol)) * portionDouble / 100);
            taco.Carboidrato = (taco.GetValue(nameof(taco.Carboidrato)) * portionDouble) / 100;
            taco.FibraAlimentar = (taco.GetValue(nameof(taco.FibraAlimentar)) * portionDouble / 100);
            taco.Cinzas = (taco.GetValue(nameof(taco.Cinzas)) * portionDouble / 100);
            taco.Calcio = (taco.GetValue(nameof(taco.Calcio)) * portionDouble / 100);
            taco.Magnesio = (taco.GetValue(nameof(taco.Magnesio)) * portionDouble / 100);
            taco.Manganes = (taco.GetValue(nameof(taco.Manganes)) * portionDouble / 100);
            taco.Fosforo = (taco.GetValue(nameof(taco.Fosforo)) * portionDouble / 100);
            taco.Ferro = (taco.GetValue(nameof(taco.Ferro)) * portionDouble / 100);
            taco.Sodio = (taco.GetValue(nameof(taco.Sodio)) * portionDouble / 100);
            taco.Potassio = (taco.GetValue(nameof(taco.Potassio)) * portionDouble / 100);
            taco.Cobre = (taco.GetValue(nameof(taco.Cobre)) * portionDouble / 100);
            taco.Zinco = (taco.GetValue(nameof(taco.Zinco)) * portionDouble / 100);
            taco.Retinol = (taco.GetValue(nameof(taco.Retinol)) * portionDouble / 100);
            taco.RE = (taco.GetValue(nameof(taco.RE)) * portionDouble / 100);
            taco.RAE = (taco.GetValue(nameof(taco.RAE)) * portionDouble / 100);
            taco.Tiamina = (taco.GetValue(nameof(taco.Tiamina)) * portionDouble / 100);
            taco.Riboflavina = (taco.GetValue(nameof(taco.Riboflavina)) * portionDouble / 100);
            taco.Piridoxina = (taco.GetValue(nameof(taco.Piridoxina)) * portionDouble / 100);
            taco.Niacina = (taco.GetValue(nameof(taco.Niacina)) * portionDouble / 100);
            taco.VitaminaC = (taco.GetValue(nameof(taco.VitaminaC)) * portionDouble / 100);

            return taco;
        }
    }
}
