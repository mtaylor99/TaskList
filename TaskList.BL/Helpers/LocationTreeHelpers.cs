using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TaskList.DAL.Models;

namespace TaskList.BL.Helpers
{
    internal static class TreeHelpers
    {
        public static IEnumerable<LocationTreeNode> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> id_selector,
            Func<T, K> parent_id_selector,
            K root_id = default(K))
        {
            foreach (var c in collection.Where(c => parent_id_selector(c).Equals(root_id)))
            {
                yield return new LocationTreeNode
                {
                    Text = (c as Location).Description,
                    HRef = (c as Location).LocationId,
                    Nodes = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }
    }

    public class LocationTreeNode
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("href")]
        public int HRef { get; set; }

        [JsonProperty("checked")]
        public bool Checked { get; set; }

        [JsonProperty("nodes")]
        public IEnumerable<LocationTreeNode> Nodes { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }
    }

    public class State
    {
        [JsonProperty("checked")]
        public bool Checked { get; set; }

        [JsonProperty("expanded")]
        public bool Expanded { get; set; }
    }
}
