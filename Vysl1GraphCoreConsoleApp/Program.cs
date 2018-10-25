using System;
using System.Text;
using System.Linq;
using C = System.Console;
using System.Collections.Generic;

namespace Vysl1GraphCoreConsoleApp
{
    class Program
    {
        private static int INITIAL_RESOURCES = 500;

        static void Main(string[] args)
        {
            TreeNode<ResourceCostPair> root = GetTree();
            C.WriteLine("Greedy algorithm by cost (nodes with the lowest cost first):");
            var res = GreedySearchByCost(root, INITIAL_RESOURCES);
            C.WriteLine($"Resources collected: {res.resourcesCollected}, resources for travel remaining: {res.resourcesForTravelRemaining}");

            C.WriteLine($"\n{new string('=', 100)}\n");

            root = GetTree();
            C.WriteLine("Greedy algorithm by resource (nodes with the highest resource first):");
            res = GreedySearchByResource(root, INITIAL_RESOURCES);
            C.WriteLine($"Resources collected: {res.resourcesCollected}, resources for travel remaining: {res.resourcesForTravelRemaining}");

            C.ReadKey();
        }

        private static (int resourcesCollected, int resourcesForTravelRemaining) 
            GreedySearchByCost(TreeNode<ResourceCostPair> root, int resourcesForTravel)
        {
            int resourcesCollected = 0;

            var nodesWithChildren = new List<TreeNode<ResourceCostPair>>();
            TreeNode<ResourceCostPair> node = root;

            while (true)
            {

                if (resourcesForTravel >= node.Data.CostToNode)
                {
                    resourcesForTravel -= node.Data.CostToNode;
                    resourcesCollected += node.Data.ResourceValue;
                    node.WasVisited = true;
                    
                    C.WriteLine($"Visiting node on level: {node.Level}, resource={node.Data.ResourceValue}, cost={node.Data.CostToNode}; " +
                        $"current fuel={resourcesForTravel}, resources collected={resourcesCollected}");
                    
                    if (!node.IsLeaf && !nodesWithChildren.Contains(node))
                    {
                        nodesWithChildren.Add(node);
                    }

                    // select unvisited child with lowest cost
                    var childrenWithLowestCost = nodesWithChildren
                        .Select(n =>
                            n.Children.Where(c => !c.WasVisited)
                                .OrderBy(c => c.Data.CostToNode)
                                .FirstOrDefault())
                        .Where(n => n != null)
                        .ToList();


                    var unvisitedNodeWithLowestCost = childrenWithLowestCost
                        .OrderBy(c => c.Data.CostToNode)
                        .FirstOrDefault();

                    // whole tree was visited -> end
                    if (unvisitedNodeWithLowestCost == null)
                        break;

                    node = unvisitedNodeWithLowestCost;
                }
                else
                {
                    break;
                }
            }

            return (resourcesCollected, resourcesForTravel);
        }

        private static (int resourcesCollected, int resourcesForTravelRemaining)
            GreedySearchByResource(TreeNode<ResourceCostPair> root, int resourcesForTravel)
        {
            int resourcesCollected = 0;

            var nodesWithChildren = new List<TreeNode<ResourceCostPair>>();
            TreeNode<ResourceCostPair> node = root;

            while (true)
            {
                if (resourcesForTravel >= node.Data.CostToNode)
                {
                    resourcesForTravel -= node.Data.CostToNode;
                    resourcesCollected += node.Data.ResourceValue;
                    node.WasVisited = true;

                    C.WriteLine($"Visiting node on level: {node.Level}, resource={node.Data.ResourceValue}, cost={node.Data.CostToNode}; " +
                        $"current fuel={resourcesForTravel}, resources collected={resourcesCollected}");

                    if (!node.IsLeaf && !nodesWithChildren.Contains(node))
                    {
                        nodesWithChildren.Add(node);
                    }

                    // select unvisited child with lowest cost
                    var childrenWithHighestResource = nodesWithChildren
                        .Select(n =>
                            n.Children
                                .Where(c => !c.WasVisited && c.Data.CostToNode <= resourcesForTravel)
                                .OrderByDescending(c => c.Data.ResourceValue)
                                .FirstOrDefault())
                        .Where(n => n != null)
                        .ToList();


                    var unvisitedNodeWithHighestResource = childrenWithHighestResource
                        .OrderByDescending(c => c.Data.ResourceValue)
                        .FirstOrDefault();

                    // whole tree was visited -> end
                    if (unvisitedNodeWithHighestResource == null)
                        break;

                    node = unvisitedNodeWithHighestResource;
                }
                else
                {
                    break;
                }
            }

            return (resourcesCollected, resourcesForTravel);
        }

        private static string CreateIndent(int depth)
        {
            return new string(' ', depth*2);
        }

        /// <summary>
        /// Vrátí strom podle zadání
        /// </summary>
        static TreeNode<ResourceCostPair> GetTree()
        {
            ResourceCostPair rootEl = new ResourceCostPair(5);
            TreeNode<ResourceCostPair> root = new TreeNode<ResourceCostPair>(rootEl);
            {
                var c1 = root.AddChild(new ResourceCostPair(14, 132));
                {
                    var c11 = c1.AddChild(new ResourceCostPair(10, 48));
                    var c12 = c1.AddChild(new ResourceCostPair(15, 21));
                }

                var c2 = root.AddChild(new ResourceCostPair(40, 145));
                {
                    var c21 = c2.AddChild(new ResourceCostPair(1, 4));
                    var c22 = c2.AddChild(new ResourceCostPair(3, 30));
                    var c23 = c2.AddChild(new ResourceCostPair(4, 78));
                    {
                        var c231 = c23.AddChild(new ResourceCostPair(19, 57));
                        var c322 = c23.AddChild(new ResourceCostPair(20, 30));
                    }
                }

                var c3 = root.AddChild(new ResourceCostPair(1, 193));
                {
                    var c31 = c3.AddChild(new ResourceCostPair(5, 12));
                    var c32 = c3.AddChild(new ResourceCostPair(12, 150));
                    {
                        var c321 = c32.AddChild(new ResourceCostPair(43, 23));
                        var c322 = c32.AddChild(new ResourceCostPair(36, 27));
                    }
                }
            }

            return root;
        }
    }
}
