using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaskList.BL.Domain;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.Web.Controllers
{
    public class TreeViewController : Controller
    {
        private ILogger<TreeViewController> logger;
        private ILocationRepository repository;
        Locations locations;

        public TreeViewController(ILogger<TreeViewController> log, ILocationRepository repo)
        {
            logger = log;
            repository = repo;
            locations = new Locations(logger, repository);

            logger.LogInformation("Locations Controller");
        }


        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetTree()
        {
            var _locations = locations.GetAllLocations();

            //IEnumerable<Location> mlc = GenerateTree(_locations,
            //                             l => l.LocationId,
            //                             l => l.ParentId,
            //                             (l, ci) => new Location
            //                             {
            //                                 LocationId = l.LocationId,
            //                                 Description = l.Description,
            //                                 ParentId = l.ParentId,
            //                                 Nodes = ci
            //                             });

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

        //public static IEnumerable<TJ> GenerateTree<T, TK, TJ>(this IEnumerable<T> items,
        //                                              Func<T, TK> idSelector,
        //                                              Func<T, TK> parentSelector,
        //                                              Func<T, IEnumerable<T>, TJ> outSelector)
        //{
        //    IList<T> mlist = items.ToList();

        //    ILookup<TK, T> mcl = mlist.ToLookup(parentSelector);

        //    return mlist.Select(cat => outSelector(cat, mcl[idSelector(cat)]));
        //}
    }
}