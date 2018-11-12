﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarPathFinder.auxiliary
{
	public class PrioritizedQueue<T>
	{

		private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();
		public List<Tuple<T, double>> Elements
		{
			//public List<T> Elements => elements.Select(x=>x.Item1).ToList();
			get => elements;
			private set => elements = value;
		}

		public int Count
		{
			get { return elements.Count; }
		}

		public void Enqueue(T item, double priority)
		{
			elements.Add(Tuple.Create(item, priority));
		}

		public T Dequeue()
		{
			int bestIndex = 0;

			for (int i = 0; i < elements.Count; i++)
			{
				if (elements[i].Item2 < elements[bestIndex].Item2)
				{
					bestIndex = i;
				}
			}

			T bestItem = elements[bestIndex].Item1;
			elements.RemoveAt(bestIndex);
			return bestItem;
		}
	}
}
