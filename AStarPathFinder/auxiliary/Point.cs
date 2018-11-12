using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarPathFinder.auxiliary
{
	public class Point
	{
		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override int GetHashCode()
		{
			return this.X * 10000 + this.Y;
		}

		public override bool Equals(object o)
		{

			var point = o as Point;

			if (point == null)
			{
				return false;
			}

			if (this.X == point.X && this.Y == point.Y) return true;
			return false;
		}

		public Point()
		{
		}

		public int X;
		public int Y;

		public static double operator -(Point a, Point b)
		{
			return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
		}
	}
}
