using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NutriLensClassLibrary.Models
{
    public static class JsonToFoodItemsParser
    {
        public static List<SimpleFoodItem> Parse(string input)
        {
            try
            {
                // Normaliza a entrada para maiúsculo
                input = input.ToUpper();

                // Cria um vetor a partir das strings "ITEM"
                string[] inputSplit = input.Split("ITEM");

                // Lista que irá receber os splits de "ITEM" e "QUANTIDADE"
                List<string> entitiesAndQuantitySplited = new();

                // Realiza o split de "QUANTIDADE" e insere na lista
                foreach (string s in inputSplit)
                {
                    string[] quantitySplit = s.Split("QUANTIDADE");

                    foreach (string qSplit in quantitySplit)
                    {
                        entitiesAndQuantitySplited.Add(qSplit);
                    }
                }

                // Regex que detecta trechos de palavras e números (para o nome do alimento)
                Regex wordsNumbersRegex = new(@"[\w\dÀ-Ü]+[\w\d À-Ü]+", RegexOptions.IgnoreCase);

                // Regex que detecta números apenas (para as gramas do alimento)
                Regex numbersRegex = new(@"[\d]+");

                // Variável que indica se o que deveria ser verificado no momento é uma palavra (nome do alimento) ou
                // número (quantidade do alimento)
                bool wordTurn = true;

                List<string> nameAndQuantitiesOnlyList = new List<string>();

                foreach (string s in entitiesAndQuantitySplited)
                {
                    string match = wordsNumbersRegex.Match(s).Value;

                    if (!string.IsNullOrEmpty(match))
                    {
                        if (wordTurn)
                        {
                            nameAndQuantitiesOnlyList.Add(match);
                            wordTurn = false;
                            continue;
                        }

                        if (numbersRegex.IsMatch(match) && int.TryParse(numbersRegex.Match(match).Value, out int value))
                        {
                            nameAndQuantitiesOnlyList.Add(numbersRegex.Match(match).Value);
                            wordTurn = true;
                        }
                        else
                        {
                            if (!wordTurn)
                            {
                                nameAndQuantitiesOnlyList.Clear();
                                nameAndQuantitiesOnlyList.Add(match);
                            }
                        }
                    }
                }

                // Lista dos alimentos
                List<SimpleFoodItem> foodItems = new List<SimpleFoodItem>();

                // A cada dois itens da lista 'nameAndQuantitiesOnlyList' adiciona um novo 'FoodItem' 
                for (int i = 0; i < nameAndQuantitiesOnlyList.Count; i += 2)
                {
                    SimpleFoodItem foodItem = new(nameAndQuantitiesOnlyList[i], int.Parse(nameAndQuantitiesOnlyList[i + 1]));
                    foodItems.Add(foodItem);
                }

                // Lista de alimentos preenchida
                return foodItems;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
