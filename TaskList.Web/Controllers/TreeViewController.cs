using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TaskList.Web.Controllers
{
    public class TreeViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetTree()
        {
            var nodes = new List<TreeNode>();

            nodes.Add(new TreeNode { Text = "Parent 1", HRef = 1, State = new State { Checked = true } });
            nodes.Add(new TreeNode { Text = "Parent 2", HRef = 2, Checked = true });
            nodes.Add(new TreeNode { Text = "Parent 3", HRef = 3, Checked = true });

            var parent4 = new TreeNode { Text = "Parent 4", HRef = 41, State = new State { Expanded = false }, Nodes = new List<TreeNode>() };
            parent4.Nodes.Add(new TreeNode { Text = "Child 1", HRef = 5 });
            parent4.Nodes.Add(new TreeNode { Text = "Child 2", HRef = 6 });
            nodes.Add(parent4);

            return Json(nodes);
        }

        public class TreeNode
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("href")]
            public int HRef { get; set; }

            [JsonProperty("checked")]
            public bool Checked { get; set; }

            [JsonProperty("nodes")]
            public List<TreeNode> Nodes { get; set; }

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
}