using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006C5 RID: 1733
	public class DirectedAcyclicGraph
	{
		// Token: 0x060036EE RID: 14062 RVA: 0x00160604 File Offset: 0x0015E804
		public DirectedAcyclicGraph(int numVertices)
		{
			this.numVertices = numVertices;
			for (int i = 0; i < numVertices; i++)
			{
				this.adjacencyList.Add(new List<int>());
			}
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x00160645 File Offset: 0x0015E845
		public void AddEdge(int from, int to)
		{
			this.adjacencyList[from].Add(to);
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x0016065C File Offset: 0x0015E85C
		public List<int> TopologicalSort()
		{
			bool[] array = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
			}
			List<int> result = new List<int>();
			for (int j = 0; j < this.numVertices; j++)
			{
				if (!array[j])
				{
					this.TopologicalSortInner(j, array, result);
				}
			}
			return result;
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x001606B0 File Offset: 0x0015E8B0
		private void TopologicalSortInner(int v, bool[] visited, List<int> result)
		{
			visited[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num])
				{
					this.TopologicalSortInner(num, visited, result);
				}
			}
			result.Add(v);
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x0016071C File Offset: 0x0015E91C
		public bool IsCyclic()
		{
			return this.FindCycle() != -1;
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x0016072C File Offset: 0x0015E92C
		public int FindCycle()
		{
			bool[] array = new bool[this.numVertices];
			bool[] array2 = new bool[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = false;
				array2[i] = false;
			}
			for (int j = 0; j < this.numVertices; j++)
			{
				if (this.IsCyclicInner(j, array, array2))
				{
					return j;
				}
			}
			return -1;
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x0016078C File Offset: 0x0015E98C
		private bool IsCyclicInner(int v, bool[] visited, bool[] history)
		{
			visited[v] = true;
			history[v] = true;
			foreach (int num in this.adjacencyList[v])
			{
				if (!visited[num] && this.IsCyclicInner(num, visited, history))
				{
					return true;
				}
				if (history[num])
				{
					return true;
				}
			}
			history[v] = false;
			return false;
		}

		// Token: 0x0400241F RID: 9247
		private int numVertices;

		// Token: 0x04002420 RID: 9248
		private List<List<int>> adjacencyList = new List<List<int>>();
	}
}
