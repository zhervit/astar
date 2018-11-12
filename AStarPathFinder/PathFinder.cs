using AStarPathFinder.auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AStarPathFinder
{
	/// <summary>
	/// Implementation of A*. Returns the next step for the player to move to the nearest target.
	/// </summary>
	public class PathFinder
	{
		/// <summary>
		/// Output to console the next step and map with path
		/// </summary>
		public bool enableConsoleLog = false;
		public Point target = new Point(5, 5);
		/// <summary>
		/// Returns the next step to go to the nearest target
		/// </summary>
		/// <param name="binMap">Binary map of environment,true - available cell, false - forbidden cell</param>
		/// <param name="player">Position of the main character</param>
		/// <param name="targets">List of targets to pursue</param>
		/// <returns></returns>
		public Point FindNextPath(int[,] binMap, Point player, List<Point> targets)
		{
			var enemiesNumber = targets.Count;

			for (int i = 0; i < enemiesNumber; i++)
			{
				target = GetNearestTarget(targets, player);
				targets.Remove(target);

				ASTAR(binMap, player, target);

				var current = target;

				var path = new List<Point> { current };
				try
				{
					while (!Equals(current, player))
					{
						current = cameFrom[current];
						path.Add(current);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					continue;
				}

				path.Reverse();

				var myNextStep = path.FirstOrDefault();
				path.Remove(myNextStep);

				ShowPathMap(binMap, path, player, target);

				if (path.Any())
				{
					myNextStep = path.FirstOrDefault();
					path.Remove(myNextStep);
				}

				if (enableConsoleLog) Console.WriteLine("{0}====>{1}", player.X + ";" + player.Y, myNextStep.X + ";" + myNextStep.Y);
				return myNextStep;

			}

			return player;
		}


		public Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
		public Dictionary<Point, double> costSoFar = new Dictionary<Point, double>();

		protected virtual void ShowPathMap(int[,] binMap, List<Point> pathoptions, Point myself, Point target)
		{
			if (enableConsoleLog)
				for (int j = 0; j < Math.Sqrt(binMap.Length); j++)
				{
					for (int i = 0; i < Math.Sqrt(binMap.Length); i++)
					{

						string cell = " ";

						if (0 == binMap[i, j])
						{
							cell = "#";
						}

						if (myself.X == i && myself.Y == j)
						{
							cell = "8";
						}
						else if (target.X == i && target.Y == j)
						{
							cell = "*";
						}
						else
						{
							var step = new Point(i, j);

							if (pathoptions.Contains(step))
							{
								cell = "1";
							}

						}


						Console.Write(cell);
					}

					Console.WriteLine();
				}
		}

		private void ASTAR(int[,] binMap, Point myself, Point target)
		{
			//var closed = new List<Point>();
			int counter = 0;
			var frontier = new PrioritizedQueue<Point>();
			frontier.Enqueue(myself, 0);

			cameFrom[myself] = myself;
			costSoFar[myself] = 0;

			while (frontier.Count > 0)
			{
				counter++;
				var current = frontier.Dequeue();

				if ((current - target) < 1)
				{
					break;
				}

				var neighbours = GetNeighborhood(binMap, current);

				foreach (var next in neighbours)
				{
					double newCost = costSoFar[current] + 1;
					if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
					{
						costSoFar[next] = newCost;
						double priority = newCost + Heuristic(next, target);//, binMap[next.X,next.Y]);		//untested weights improvement
						frontier.Enqueue(next, priority);
						cameFrom[next] = current;
					}
				}
			}

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="evaluatingCell">Current cell</param>
		/// <param name="targetCell">Target cell</param>
		/// <param name="weightOfCurrentCell">1 by default, Can not be 0, 1-best option, 2 a little bit worse, etc.</param>
		/// <returns></returns>
		public virtual double Heuristic(Point evaluatingCell, Point targetCell, int weightOfCurrentCell = 1)
		{
			return (Math.Abs(evaluatingCell.X - targetCell.X) + Math.Abs(evaluatingCell.Y - targetCell.Y)) * weightOfCurrentCell;
		}

		private List<Point> GetNeighborhood(int[,] binMap, Point myself)
		{
			var neighbors = new List<Point>();

			if (binMap[myself.X - 1, myself.Y] >= 1)
				neighbors.Add(new Point(myself.X - 1, myself.Y));

			if (binMap[myself.X + 1, myself.Y] >= 1)
				neighbors.Add(new Point(myself.X + 1, myself.Y));

			if (binMap[myself.X, myself.Y - 1] >= 1)
				neighbors.Add(new Point(myself.X, myself.Y - 1));

			if (binMap[myself.X, myself.Y + 1] >= 1)
				neighbors.Add(new Point(myself.X, myself.Y + 1));


			return neighbors;
		}


		private static Point GetNearestTarget(List<Point> enemies, Point myself)
		{
			double closestDistance = double.MaxValue;//over 9000 ...
			var closestPoint = new Point();

			foreach (var enemy in enemies)
			{
				if (Math.Abs(enemy - myself) < closestDistance)
				{
					closestDistance = Math.Abs(enemy - myself);
					closestPoint = enemy;
				}
			}

			return closestPoint;
		}
	}

}
