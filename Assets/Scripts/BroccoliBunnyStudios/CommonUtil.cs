using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BroccoliBunnyStudios
{
    public static class CommonUtil
    {
        /// <summary>
        /// Parse string to list of string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="delimiter"></param>
        /// <returns>List<string></string></returns>
        public static List<string> ParseToStrList(string data, char delimiter)
        {
            List<string> list = new();

            string[] elements = data.Split(delimiter);
            foreach (string element in elements)
            {
                list.Add(element);
            }

            return list;
        }

        /// <summary>
        /// Parse string to list of int
        /// </summary>
        /// <param name="data"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static List<int> ParseToIntList(string data, char delimiter)
        {
            List<int> list = new();

            int number = 0;
            string[] elements = data.Split(delimiter);
            foreach (string element in elements)
            {
                if (int.TryParse(element, out number))
                {
                    list.Add(number);
                }
            }

            return list;
        }

        /// <summary>
        /// Randomize a number to judge whether is it sucessful
        /// </summary>
        /// <param name="successPer"></param>
        /// <returns></returns>
        public static bool JudgeExe(float successPer)
        {
            float randomValue = Random.Range(0f, 100f + Mathf.Epsilon);

            return (randomValue < successPer);
        }

        /// <summary>
        /// Get nearest target
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemyList"></param>
        /// <returns>GameObject</returns>
        public static GameObject NearestTarget(GameObject player, GameObject[] enemyList, float distance)
        {
            List<GameObject> withinRangeList = TargetInRange(player, enemyList, distance);

            GameObject nearestTarget = null;
            float shortestDistance = Mathf.Infinity;

            // Loop through each object found and log its name
            foreach (GameObject enemy in withinRangeList)
            {
                // For checking closest enemy from enemy
                if (enemy == player)
                {
                    continue;
                }
                // Check if this target is closer than the current nearest one
                float distanceToTarget = Vector3.Distance(player.transform.position, enemy.transform.position);
                if (distanceToTarget < distance && enemy.activeSelf)
                {
                    shortestDistance = distanceToTarget;
                    nearestTarget = enemy;
                }
            }

            return nearestTarget;
        }

        /// <summary>
        /// Get random target
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemyList"></param>
        /// <returns>GameObject</returns>
        public static GameObject RandomTarget(GameObject player, GameObject[] enemyList, float distance)
        {
            List<GameObject> withinRangeList = TargetInRange(player, enemyList, distance);
            if (withinRangeList == null)
            {
                return null;
            }

            int count = withinRangeList.Count;
            int index = Random.Range(0, count);

            if (index < count)
            {
                return withinRangeList[index];
            }

            return null;
        }


        public static List<GameObject> TargetInRange(GameObject player, GameObject[] enemyList, float distance)
        {
            List<GameObject> foundList = new List<GameObject>();

            // Loop through each object found and log its name
            foreach (GameObject enemy in enemyList)
            {
                // Check if this target is closer than the current nearest one
                float distanceToTarget = Vector3.Distance(player.transform.position, enemy.transform.position);
                if (distanceToTarget < distance && enemy.activeSelf)
                {
                    foundList.Add(enemy);
                }
            }

            return foundList;
        }

        /// <summary>
        /// Get component from gameobject with search child option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="isSearchChild"></param>
        /// <returns>T</returns>
        public static T GetComponent<T>(GameObject gameObject, bool isSearchChild = true) where T : Component
        {
            if (gameObject != null)
            {
                if (gameObject.TryGetComponent<T>(out var component))
                {
                    return component;
                }

                if (isSearchChild)
                {
                    return gameObject.GetComponentInChildren<T>();
                }
            }

            return null;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Save AssetDatabase
        /// </summary>
        /// <param name="obj"></param>
        public static void SaveScriptableObject(UnityEngine.Object obj)
        {
            // Mark the ScriptableObject as dirty
            UnityEditor.EditorUtility.SetDirty(obj);

            // Save the changes to the asset file
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("ScriptableObject updated and saved!");

            // You can optionally create a new asset or overwrite the existing one
            // For example, save it as a new asset
            //string path = "Assets/GameAssets/data_based/data/DWorldReward.asset";
            //UnityEditor.AssetDatabase.CreateAsset(obj, path);
            //UnityEditor.AssetDatabase.SaveAssets();
            //UnityEditor.AssetDatabase.Refresh();
            //Debug.Log("ScriptableObject saved at: " + path);
        }
#endif

        /// <summary>
        /// Remove double quote in line
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string</returns>
        public static string RemoveMultipleNewlinesInQuotes(string input)
        {
            return Regex.Replace(input, "\"([^\"]*?)(\n+)([^\"]*?)\"", m =>
                $"\"{m.Groups[1].Value} {m.Groups[3].Value}\"");
        }

        /// <summary>
        /// Calculate value from percentage
        /// </summary>
        /// <param name="per"></param>
        /// <param name="value"></param>
        /// <returns>float</returns>
        public static float CalValue(float per, float value)
        {
            return per * value / 100;
        }

        /// <summary>
        /// Calculate percentage
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <returns>float</returns>
        public static float CalPer(float value, float maxValue)
        {
            return value * 100 / maxValue;
        }

        /// <summary>
        /// Convert string to int
        /// </summary>
        /// <param name="text"></param>
        /// <returns>int</returns>
        public static int ConvertToInt32(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            int result;
            if (int.TryParse(text, out result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Convert string to float
        /// </summary>
        /// <param name="text"></param>
        /// <returns>float</returns>
        public static float ConvertToSingle(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            float result;
            if (float.TryParse(text, out result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Convert string to double
        /// </summary>
        /// <param name="text"></param>
        /// <returns>double</returns>
        public static double ConvertToDouble(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            double result;
            if (double.TryParse(text, out result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Convert string to decimal
        /// </summary>
        /// <param name="text"></param>
        /// <returns>decimal</returns>
        public static decimal ConvertToDecimal(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            decimal result;
            if (decimal.TryParse(text, out result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Given a list of weights, return one of them at random treating the data as weights.
        /// Return value of -1 means invalid input (nothing to random from or no positive weights).
        /// </summary>
        public static int GetRandomIndexByWeight(List<int> weights)
        {
            if (weights == null || weights.Count == 0)
            {
                return -1;
            }

            // Calculate total weight, ignore values <= 0
            var totalWeight = 0;
            foreach (var weight in weights)
            {
                if (weight > 0)
                {
                    totalWeight += weight;
                }
            }

            if (totalWeight <= 0)
            {
                // Nothing with positive chance to random from
                return -1;
            }

            // Roll a random number from [1 to totalWeight] inclusive
            var randomValue = Random.Range(1, totalWeight + 1);

            // Select based on the weight, ignore values <= 0
            var cumulativeWeight = 0;
            for (var i = 0; i < weights.Count; i++)
            {
                var weight = weights[i];
                if (weight > 0)
                {
                    cumulativeWeight += weight;
                    if (randomValue <= cumulativeWeight)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Given a list of weights, return one of them at random treating the data as weights.
        /// Return value of -1 means invalid input (nothing to random from or no positive weights).
        /// </summary>
        public static int GetRandomIndexByWeight(List<float> weights)
        {
            if (weights == null || weights.Count == 0)
            {
                return -1;
            }

            // Calculate total weight, ignore values <= 0
            var totalWeight = 0f;
            foreach (var weight in weights)
            {
                if (weight > 0f)
                {
                    totalWeight += weight;
                }
            }

            if (totalWeight <= 0f)
            {
                // Nothing with positive chance to random from
                return -1;
            }

            // Roll a random number from [1 to totalWeight] inclusive
            var randomValue = Random.Range(0f, totalWeight);

            // Select based on the weight, ignore values <= 0
            var cumulativeWeight = 0f;
            for (var i = 0; i < weights.Count; i++)
            {
                var weight = weights[i];
                if (weight > 0)
                {
                    cumulativeWeight += weight;
                    if (randomValue <= cumulativeWeight)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}