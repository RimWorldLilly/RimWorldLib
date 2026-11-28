using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x0200242B RID: 9259
	public class Delaunator
	{
		// Token: 0x1700292A RID: 10538
		// (get) Token: 0x0600E86B RID: 59499 RVA: 0x00566FC2 File Offset: 0x005651C2
		// (set) Token: 0x0600E86C RID: 59500 RVA: 0x00566FCA File Offset: 0x005651CA
		public int[] Triangles { get; private set; }

		// Token: 0x1700292B RID: 10539
		// (get) Token: 0x0600E86D RID: 59501 RVA: 0x00566FD3 File Offset: 0x005651D3
		// (set) Token: 0x0600E86E RID: 59502 RVA: 0x00566FDB File Offset: 0x005651DB
		public int[] Halfedges { get; private set; }

		// Token: 0x1700292C RID: 10540
		// (get) Token: 0x0600E86F RID: 59503 RVA: 0x00566FE4 File Offset: 0x005651E4
		// (set) Token: 0x0600E870 RID: 59504 RVA: 0x00566FEC File Offset: 0x005651EC
		public Vector2[] Points { get; private set; }

		// Token: 0x1700292D RID: 10541
		// (get) Token: 0x0600E871 RID: 59505 RVA: 0x00566FF5 File Offset: 0x005651F5
		// (set) Token: 0x0600E872 RID: 59506 RVA: 0x00566FFD File Offset: 0x005651FD
		public int[] Hull { get; private set; }

		// Token: 0x0600E873 RID: 59507 RVA: 0x00567008 File Offset: 0x00565208
		public Delaunator(Vector2[] points)
		{
			if (points.Length < 3)
			{
				throw new ArgumentOutOfRangeException("Need at least 3 points");
			}
			this.Points = points;
			this.coords = new double[this.Points.Length * 2];
			for (int i = 0; i < this.Points.Length; i++)
			{
				Vector2 vector = this.Points[i];
				this.coords[2 * i] = (double)vector.x;
				this.coords[2 * i + 1] = (double)vector.y;
			}
			int num = points.Length;
			int num2 = 2 * num - 5;
			this.Triangles = new int[num2 * 3];
			this.Halfedges = new int[num2 * 3];
			this.hashSize = (int)Math.Ceiling(Math.Sqrt((double)num));
			this.hullPrev = new int[num];
			this.hullNext = new int[num];
			this.hullTri = new int[num];
			this.hullHash = new int[this.hashSize];
			int[] array = new int[num];
			double num3 = double.PositiveInfinity;
			double num4 = double.PositiveInfinity;
			double num5 = double.NegativeInfinity;
			double num6 = double.NegativeInfinity;
			for (int j = 0; j < num; j++)
			{
				double num7 = this.coords[2 * j];
				double num8 = this.coords[2 * j + 1];
				if (num7 < num3)
				{
					num3 = num7;
				}
				if (num8 < num4)
				{
					num4 = num8;
				}
				if (num7 > num5)
				{
					num5 = num7;
				}
				if (num8 > num6)
				{
					num6 = num8;
				}
				array[j] = j;
			}
			double ax = (num3 + num5) / 2.0;
			double ay = (num4 + num6) / 2.0;
			double num9 = double.PositiveInfinity;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			for (int k = 0; k < num; k++)
			{
				double num13 = Delaunator.Dist(ax, ay, this.coords[2 * k], this.coords[2 * k + 1]);
				if (num13 < num9)
				{
					num10 = k;
					num9 = num13;
				}
			}
			double num14 = this.coords[2 * num10];
			double num15 = this.coords[2 * num10 + 1];
			num9 = double.PositiveInfinity;
			for (int l = 0; l < num; l++)
			{
				if (l != num10)
				{
					double num16 = Delaunator.Dist(num14, num15, this.coords[2 * l], this.coords[2 * l + 1]);
					if (num16 < num9 && num16 > 0.0)
					{
						num11 = l;
						num9 = num16;
					}
				}
			}
			double num17 = this.coords[2 * num11];
			double num18 = this.coords[2 * num11 + 1];
			double num19 = double.PositiveInfinity;
			for (int m = 0; m < num; m++)
			{
				if (m != num10 && m != num11)
				{
					double num20 = Delaunator.Circumradius(num14, num15, num17, num18, this.coords[2 * m], this.coords[2 * m + 1]);
					if (num20 < num19)
					{
						num12 = m;
						num19 = num20;
					}
				}
			}
			double num21 = this.coords[2 * num12];
			double num22 = this.coords[2 * num12 + 1];
			if (num19 == double.PositiveInfinity)
			{
				throw new Exception("No Delaunay triangulation exists for this input.");
			}
			if (Delaunator.Orient(num14, num15, num17, num18, num21, num22))
			{
				int num23 = num11;
				double num24 = num17;
				double num25 = num18;
				num11 = num12;
				num17 = num21;
				num18 = num22;
				num12 = num23;
				num21 = num24;
				num22 = num25;
			}
			Vector2 vector2 = Delaunator.Circumcenter(num14, num15, num17, num18, num21, num22);
			this.cx = (double)vector2.x;
			this.cy = (double)vector2.y;
			double[] array2 = new double[num];
			for (int n = 0; n < num; n++)
			{
				array2[n] = Delaunator.Dist(this.coords[2 * n], this.coords[2 * n + 1], (double)vector2.x, (double)vector2.y);
			}
			Delaunator.Quicksort(array, array2, 0, num - 1);
			this.hullStart = num10;
			this.hullSize = 3;
			this.hullNext[num10] = (this.hullPrev[num12] = num11);
			this.hullNext[num11] = (this.hullPrev[num10] = num12);
			this.hullNext[num12] = (this.hullPrev[num11] = num10);
			this.hullTri[num10] = 0;
			this.hullTri[num11] = 1;
			this.hullTri[num12] = 2;
			this.hullHash[this.HashKey(num14, num15)] = num10;
			this.hullHash[this.HashKey(num17, num18)] = num11;
			this.hullHash[this.HashKey(num21, num22)] = num12;
			this.trianglesLen = 0;
			this.AddTriangle(num10, num11, num12, -1, -1, -1);
			double num26 = 0.0;
			double num27 = 0.0;
			for (int num28 = 0; num28 < array.Length; num28++)
			{
				int num29 = array[num28];
				double num30 = this.coords[2 * num29];
				double num31 = this.coords[2 * num29 + 1];
				if (num28 <= 0 || Math.Abs(num30 - num26) > this.EPSILON || Math.Abs(num31 - num27) > this.EPSILON)
				{
					num26 = num30;
					num27 = num31;
					if (num29 != num10 && num29 != num11 && num29 != num12)
					{
						int num32 = 0;
						for (int num33 = 0; num33 < this.hashSize; num33++)
						{
							int num34 = this.HashKey(num30, num31);
							num32 = this.hullHash[(num34 + num33) % this.hashSize];
							if (num32 != -1 && num32 != this.hullNext[num32])
							{
								break;
							}
						}
						num32 = this.hullPrev[num32];
						int num35 = num32;
						int num36 = this.hullNext[num35];
						while (!Delaunator.Orient(num30, num31, this.coords[2 * num35], this.coords[2 * num35 + 1], this.coords[2 * num36], this.coords[2 * num36 + 1]))
						{
							num35 = num36;
							if (num35 == num32)
							{
								num35 = int.MaxValue;
								break;
							}
							num36 = this.hullNext[num35];
						}
						if (num35 != 2147483647)
						{
							int num37 = this.AddTriangle(num35, num29, this.hullNext[num35], -1, -1, this.hullTri[num35]);
							this.hullTri[num29] = this.Legalize(num37 + 2);
							this.hullTri[num35] = num37;
							this.hullSize++;
							int num38 = this.hullNext[num35];
							num36 = this.hullNext[num38];
							while (Delaunator.Orient(num30, num31, this.coords[2 * num38], this.coords[2 * num38 + 1], this.coords[2 * num36], this.coords[2 * num36 + 1]))
							{
								num37 = this.AddTriangle(num38, num29, num36, this.hullTri[num29], -1, this.hullTri[num38]);
								this.hullTri[num29] = this.Legalize(num37 + 2);
								this.hullNext[num38] = num38;
								this.hullSize--;
								num38 = num36;
								num36 = this.hullNext[num38];
							}
							if (num35 == num32)
							{
								num36 = this.hullPrev[num35];
								while (Delaunator.Orient(num30, num31, this.coords[2 * num36], this.coords[2 * num36 + 1], this.coords[2 * num35], this.coords[2 * num35 + 1]))
								{
									num37 = this.AddTriangle(num36, num29, num35, -1, this.hullTri[num35], this.hullTri[num36]);
									this.Legalize(num37 + 2);
									this.hullTri[num36] = num37;
									this.hullNext[num35] = num35;
									this.hullSize--;
									num35 = num36;
									num36 = this.hullPrev[num35];
								}
							}
							this.hullStart = (this.hullPrev[num29] = num35);
							this.hullNext[num35] = (this.hullPrev[num38] = num29);
							this.hullNext[num29] = num38;
							this.hullHash[this.HashKey(num30, num31)] = num29;
							this.hullHash[this.HashKey(this.coords[2 * num35], this.coords[2 * num35 + 1])] = num35;
						}
					}
				}
			}
			this.Hull = new int[this.hullSize];
			int num39 = this.hullStart;
			for (int num40 = 0; num40 < this.hullSize; num40++)
			{
				this.Hull[num40] = num39;
				num39 = this.hullNext[num39];
			}
			this.hullPrev = (this.hullNext = (this.hullTri = null));
			this.Triangles = this.Triangles.Take(this.trianglesLen).ToArray<int>();
			this.Halfedges = this.Halfedges.Take(this.trianglesLen).ToArray<int>();
		}

		// Token: 0x0600E874 RID: 59508 RVA: 0x00567904 File Offset: 0x00565B04
		private int Legalize(int a)
		{
			int num = 0;
			int num4;
			for (;;)
			{
				int num2 = this.Halfedges[a];
				int num3 = a - a % 3;
				num4 = num3 + (a + 2) % 3;
				if (num2 == -1)
				{
					if (num == 0)
					{
						break;
					}
					a = this.EDGE_STACK[--num];
				}
				else
				{
					int num5 = num2 - num2 % 3;
					int num6 = num3 + (a + 1) % 3;
					int num7 = num5 + (num2 + 2) % 3;
					int num8 = this.Triangles[num4];
					int num9 = this.Triangles[a];
					int num10 = this.Triangles[num6];
					int num11 = this.Triangles[num7];
					if (Delaunator.InCircle(this.coords[2 * num8], this.coords[2 * num8 + 1], this.coords[2 * num9], this.coords[2 * num9 + 1], this.coords[2 * num10], this.coords[2 * num10 + 1], this.coords[2 * num11], this.coords[2 * num11 + 1]))
					{
						this.Triangles[a] = num11;
						this.Triangles[num2] = num8;
						int num12 = this.Halfedges[num7];
						if (num12 == -1)
						{
							int num13 = this.hullStart;
							while (this.hullTri[num13] != num7)
							{
								num13 = this.hullPrev[num13];
								if (num13 == this.hullStart)
								{
									goto IL_13B;
								}
							}
							this.hullTri[num13] = a;
						}
						IL_13B:
						this.Link(a, num12);
						this.Link(num2, this.Halfedges[num4]);
						this.Link(num4, num7);
						int num14 = num5 + (num2 + 1) % 3;
						if (num < this.EDGE_STACK.Length)
						{
							this.EDGE_STACK[num++] = num14;
						}
					}
					else
					{
						if (num == 0)
						{
							break;
						}
						a = this.EDGE_STACK[--num];
					}
				}
			}
			return num4;
		}

		// Token: 0x0600E875 RID: 59509 RVA: 0x00567AB0 File Offset: 0x00565CB0
		private static bool InCircle(double ax, double ay, double bx, double by, double cx, double cy, double px, double py)
		{
			double num = ax - px;
			double num2 = ay - py;
			double num3 = bx - px;
			double num4 = by - py;
			double num5 = cx - px;
			double num6 = cy - py;
			double num7 = num * num + num2 * num2;
			double num8 = num3 * num3 + num4 * num4;
			double num9 = num5 * num5 + num6 * num6;
			return num * (num4 * num9 - num8 * num6) - num2 * (num3 * num9 - num8 * num5) + num7 * (num3 * num6 - num4 * num5) < 0.0;
		}

		// Token: 0x0600E876 RID: 59510 RVA: 0x00567B28 File Offset: 0x00565D28
		private int AddTriangle(int i0, int i1, int i2, int a, int b, int c)
		{
			int num = this.trianglesLen;
			this.Triangles[num] = i0;
			this.Triangles[num + 1] = i1;
			this.Triangles[num + 2] = i2;
			this.Link(num, a);
			this.Link(num + 1, b);
			this.Link(num + 2, c);
			this.trianglesLen += 3;
			return num;
		}

		// Token: 0x0600E877 RID: 59511 RVA: 0x00567B89 File Offset: 0x00565D89
		private void Link(int a, int b)
		{
			this.Halfedges[a] = b;
			if (b != -1)
			{
				this.Halfedges[b] = a;
			}
		}

		// Token: 0x0600E878 RID: 59512 RVA: 0x00567BA1 File Offset: 0x00565DA1
		private int HashKey(double x, double y)
		{
			return (int)(Math.Floor(Delaunator.PseudoAngle(x - this.cx, y - this.cy) * (double)this.hashSize) % (double)this.hashSize);
		}

		// Token: 0x0600E879 RID: 59513 RVA: 0x00567BD0 File Offset: 0x00565DD0
		private static double PseudoAngle(double dx, double dy)
		{
			double num = dx / (Math.Abs(dx) + Math.Abs(dy));
			return ((dy > 0.0) ? (3.0 - num) : (1.0 + num)) / 4.0;
		}

		// Token: 0x0600E87A RID: 59514 RVA: 0x00567C1C File Offset: 0x00565E1C
		private static void Quicksort(int[] ids, double[] dists, int left, int right)
		{
			if (right - left <= 20)
			{
				for (int i = left + 1; i <= right; i++)
				{
					int num = ids[i];
					double num2 = dists[num];
					int num3 = i - 1;
					while (num3 >= left && dists[ids[num3]] > num2)
					{
						ids[num3 + 1] = ids[num3--];
					}
					ids[num3 + 1] = num;
				}
				return;
			}
			int i2 = left + right >> 1;
			int num4 = left + 1;
			int num5 = right;
			Delaunator.Swap(ids, i2, num4);
			if (dists[ids[left]] > dists[ids[right]])
			{
				Delaunator.Swap(ids, left, right);
			}
			if (dists[ids[num4]] > dists[ids[right]])
			{
				Delaunator.Swap(ids, num4, right);
			}
			if (dists[ids[left]] > dists[ids[num4]])
			{
				Delaunator.Swap(ids, left, num4);
			}
			int num6 = ids[num4];
			double num7 = dists[num6];
			for (;;)
			{
				num4++;
				if (dists[ids[num4]] >= num7)
				{
					do
					{
						num5--;
					}
					while (dists[ids[num5]] > num7);
					if (num5 < num4)
					{
						break;
					}
					Delaunator.Swap(ids, num4, num5);
				}
			}
			ids[left + 1] = ids[num5];
			ids[num5] = num6;
			if (right - num4 + 1 >= num5 - left)
			{
				Delaunator.Quicksort(ids, dists, num4, right);
				Delaunator.Quicksort(ids, dists, left, num5 - 1);
				return;
			}
			Delaunator.Quicksort(ids, dists, left, num5 - 1);
			Delaunator.Quicksort(ids, dists, num4, right);
		}

		// Token: 0x0600E87B RID: 59515 RVA: 0x00567D4C File Offset: 0x00565F4C
		private static void Swap(int[] arr, int i, int j)
		{
			int num = arr[i];
			arr[i] = arr[j];
			arr[j] = num;
		}

		// Token: 0x0600E87C RID: 59516 RVA: 0x00567D67 File Offset: 0x00565F67
		private static bool Orient(double px, double py, double qx, double qy, double rx, double ry)
		{
			return (qy - py) * (rx - qx) - (qx - px) * (ry - qy) < 0.0;
		}

		// Token: 0x0600E87D RID: 59517 RVA: 0x00567D88 File Offset: 0x00565F88
		private static double Circumradius(double ax, double ay, double bx, double by, double cx, double cy)
		{
			double num = bx - ax;
			double num2 = by - ay;
			double num3 = cx - ax;
			double num4 = cy - ay;
			double num5 = num * num + num2 * num2;
			double num6 = num3 * num3 + num4 * num4;
			double num7 = 0.5 / (num * num4 - num2 * num3);
			double num8 = (num4 * num5 - num2 * num6) * num7;
			double num9 = (num * num6 - num3 * num5) * num7;
			return num8 * num8 + num9 * num9;
		}

		// Token: 0x0600E87E RID: 59518 RVA: 0x00567DF0 File Offset: 0x00565FF0
		private static Vector2 Circumcenter(double ax, double ay, double bx, double by, double cx, double cy)
		{
			double num = bx - ax;
			double num2 = by - ay;
			double num3 = cx - ax;
			double num4 = cy - ay;
			double num5 = num * num + num2 * num2;
			double num6 = num3 * num3 + num4 * num4;
			double num7 = 0.5 / (num * num4 - num2 * num3);
			float num8 = (float)(ax + (num4 * num5 - num2 * num6) * num7);
			double num9 = ay + (num * num6 - num3 * num5) * num7;
			return new Vector2(num8, (float)num9);
		}

		// Token: 0x0600E87F RID: 59519 RVA: 0x00567E5C File Offset: 0x0056605C
		private static double Dist(double ax, double ay, double bx, double by)
		{
			double num = ax - bx;
			double num2 = ay - by;
			return num * num + num2 * num2;
		}

		// Token: 0x0600E880 RID: 59520 RVA: 0x00567E76 File Offset: 0x00566076
		public IEnumerable<ITriangle> GetTriangles()
		{
			Delaunator.<GetTriangles>d__42 <GetTriangles>d__ = new Delaunator.<GetTriangles>d__42(-2);
			<GetTriangles>d__.<>4__this = this;
			return <GetTriangles>d__;
		}

		// Token: 0x0600E881 RID: 59521 RVA: 0x00567E86 File Offset: 0x00566086
		public IEnumerable<IEdge> GetEdges()
		{
			Delaunator.<GetEdges>d__43 <GetEdges>d__ = new Delaunator.<GetEdges>d__43(-2);
			<GetEdges>d__.<>4__this = this;
			return <GetEdges>d__;
		}

		// Token: 0x0600E882 RID: 59522 RVA: 0x00567E96 File Offset: 0x00566096
		public IEnumerable<IEdge> GetVoronoiEdges(Func<int, Vector2> triangleVerticeSelector = null)
		{
			Delaunator.<GetVoronoiEdges>d__44 <GetVoronoiEdges>d__ = new Delaunator.<GetVoronoiEdges>d__44(-2);
			<GetVoronoiEdges>d__.<>4__this = this;
			<GetVoronoiEdges>d__.<>3__triangleVerticeSelector = triangleVerticeSelector;
			return <GetVoronoiEdges>d__;
		}

		// Token: 0x0600E883 RID: 59523 RVA: 0x00567EAD File Offset: 0x005660AD
		public IEnumerable<IEdge> GetVoronoiEdgesBasedOnCircumCenter()
		{
			return this.GetVoronoiEdges(new Func<int, Vector2>(this.GetTriangleCircumcenter));
		}

		// Token: 0x0600E884 RID: 59524 RVA: 0x00567EC1 File Offset: 0x005660C1
		public IEnumerable<IEdge> GetVoronoiEdgesBasedOnCentroids()
		{
			return this.GetVoronoiEdges(new Func<int, Vector2>(this.GetCentroid));
		}

		// Token: 0x0600E885 RID: 59525 RVA: 0x00567ED5 File Offset: 0x005660D5
		public IEnumerable<IVoronoiCell> GetVoronoiCells(Func<int, Vector2> triangleVerticeSelector = null)
		{
			Delaunator.<GetVoronoiCells>d__47 <GetVoronoiCells>d__ = new Delaunator.<GetVoronoiCells>d__47(-2);
			<GetVoronoiCells>d__.<>4__this = this;
			<GetVoronoiCells>d__.<>3__triangleVerticeSelector = triangleVerticeSelector;
			return <GetVoronoiCells>d__;
		}

		// Token: 0x0600E886 RID: 59526 RVA: 0x00567EEC File Offset: 0x005660EC
		public IEnumerable<IVoronoiCell> GetVoronoiCellsBasedOnCircumcenters()
		{
			return this.GetVoronoiCells(new Func<int, Vector2>(this.GetTriangleCircumcenter));
		}

		// Token: 0x0600E887 RID: 59527 RVA: 0x00567F00 File Offset: 0x00566100
		public IEnumerable<IVoronoiCell> GetVoronoiCellsBasedOnCentroids()
		{
			return this.GetVoronoiCells(new Func<int, Vector2>(this.GetCentroid));
		}

		// Token: 0x0600E888 RID: 59528 RVA: 0x00567F14 File Offset: 0x00566114
		public IEnumerable<IEdge> GetHullEdges()
		{
			return Delaunator.CreateHull(this.GetHullPoints());
		}

		// Token: 0x0600E889 RID: 59529 RVA: 0x00567F21 File Offset: 0x00566121
		public Vector2[] GetHullPoints()
		{
			return Array.ConvertAll<int, Vector2>(this.Hull, (int x) => this.Points[x]);
		}

		// Token: 0x0600E88A RID: 59530 RVA: 0x00567F3C File Offset: 0x0056613C
		public Vector2[] GetTrianglePoints(int t)
		{
			List<Vector2> list = new List<Vector2>();
			foreach (int num in this.PointsOfTriangle(t))
			{
				list.Add(this.Points[num]);
			}
			return list.ToArray();
		}

		// Token: 0x0600E88B RID: 59531 RVA: 0x00567FA4 File Offset: 0x005661A4
		public Vector2[] GetRellaxedPoints()
		{
			List<Vector2> list = new List<Vector2>();
			foreach (IVoronoiCell voronoiCell in this.GetVoronoiCellsBasedOnCircumcenters())
			{
				list.Add(Delaunator.GetCentroid(voronoiCell.Points));
			}
			return list.ToArray();
		}

		// Token: 0x0600E88C RID: 59532 RVA: 0x00568008 File Offset: 0x00566208
		public IEnumerable<IEdge> GetEdgesOfTriangle(int t)
		{
			return Delaunator.CreateHull(from p in Delaunator.EdgesOfTriangle(t)
			select this.Points[p]);
		}

		// Token: 0x0600E88D RID: 59533 RVA: 0x00568026 File Offset: 0x00566226
		public static IEnumerable<IEdge> CreateHull(IEnumerable<Vector2> points)
		{
			return points.Zip(points.Skip(1).Append(points.FirstOrDefault<Vector2>()), (Vector2 a, Vector2 b) => new Edge(0, a, b)).OfType<IEdge>();
		}

		// Token: 0x0600E88E RID: 59534 RVA: 0x00568064 File Offset: 0x00566264
		public Vector2 GetTriangleCircumcenter(int t)
		{
			Vector2[] trianglePoints = this.GetTrianglePoints(t);
			return Delaunator.GetCircumcenter(trianglePoints[0], trianglePoints[1], trianglePoints[2]);
		}

		// Token: 0x0600E88F RID: 59535 RVA: 0x00568093 File Offset: 0x00566293
		public Vector2 GetCentroid(int t)
		{
			return Delaunator.GetCentroid(this.GetTrianglePoints(t));
		}

		// Token: 0x0600E890 RID: 59536 RVA: 0x005680A1 File Offset: 0x005662A1
		public static Vector2 GetCircumcenter(Vector2 a, Vector2 b, Vector2 c)
		{
			return Delaunator.Circumcenter((double)a.x, (double)a.y, (double)b.x, (double)b.y, (double)c.x, (double)c.y);
		}

		// Token: 0x0600E891 RID: 59537 RVA: 0x005680D4 File Offset: 0x005662D4
		public static Vector2 GetCentroid(Vector2[] points)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			int i = 0;
			int num4 = points.Length - 1;
			while (i < points.Length)
			{
				float num5 = points[i].x * points[num4].y - points[num4].x * points[i].y;
				num += (double)num5;
				num2 += (double)((points[i].x + points[num4].x) * num5);
				num3 += (double)((points[i].y + points[num4].y) * num5);
				num4 = i++;
			}
			if (Math.Abs(num) < 1.0000000116860974E-07)
			{
				return default(Vector2);
			}
			num *= 3.0;
			return new Vector2((float)(num2 / num), (float)(num3 / num));
		}

		// Token: 0x0600E892 RID: 59538 RVA: 0x005681D4 File Offset: 0x005663D4
		public void ForEachTriangle(Action<ITriangle> callback)
		{
			foreach (ITriangle obj in this.GetTriangles())
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x0600E893 RID: 59539 RVA: 0x00568224 File Offset: 0x00566424
		public void ForEachTriangleEdge(Action<IEdge> callback)
		{
			foreach (IEdge obj in this.GetEdges())
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x0600E894 RID: 59540 RVA: 0x00568274 File Offset: 0x00566474
		public void ForEachVoronoiEdge(Action<IEdge> callback)
		{
			foreach (IEdge obj in this.GetVoronoiEdges(null))
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x0600E895 RID: 59541 RVA: 0x005682C8 File Offset: 0x005664C8
		public void ForEachVoronoiCellBasedOnCentroids(Action<IVoronoiCell> callback)
		{
			foreach (IVoronoiCell obj in this.GetVoronoiCellsBasedOnCentroids())
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x0600E896 RID: 59542 RVA: 0x00568318 File Offset: 0x00566518
		public void ForEachVoronoiCellBasedOnCircumcenters(Action<IVoronoiCell> callback)
		{
			foreach (IVoronoiCell obj in this.GetVoronoiCellsBasedOnCircumcenters())
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x0600E897 RID: 59543 RVA: 0x00568368 File Offset: 0x00566568
		public void ForEachVoronoiCell(Action<IVoronoiCell> callback, Func<int, Vector2> triangleVertexSelector = null)
		{
			foreach (IVoronoiCell obj in this.GetVoronoiCells(triangleVertexSelector))
			{
				if (callback != null)
				{
					callback(obj);
				}
			}
		}

		// Token: 0x0600E898 RID: 59544 RVA: 0x005683BC File Offset: 0x005665BC
		public IEnumerable<int> EdgesAroundPoint(int start)
		{
			Delaunator.<EdgesAroundPoint>d__66 <EdgesAroundPoint>d__ = new Delaunator.<EdgesAroundPoint>d__66(-2);
			<EdgesAroundPoint>d__.<>4__this = this;
			<EdgesAroundPoint>d__.<>3__start = start;
			return <EdgesAroundPoint>d__;
		}

		// Token: 0x0600E899 RID: 59545 RVA: 0x005683D3 File Offset: 0x005665D3
		public IEnumerable<int> PointsOfTriangle(int t)
		{
			Delaunator.<PointsOfTriangle>d__67 <PointsOfTriangle>d__ = new Delaunator.<PointsOfTriangle>d__67(-2);
			<PointsOfTriangle>d__.<>4__this = this;
			<PointsOfTriangle>d__.<>3__t = t;
			return <PointsOfTriangle>d__;
		}

		// Token: 0x0600E89A RID: 59546 RVA: 0x005683EC File Offset: 0x005665EC
		public IEnumerable<int> TrianglesAdjacentToTriangle(int t)
		{
			List<int> list = new List<int>();
			foreach (int num in Delaunator.EdgesOfTriangle(t))
			{
				int num2 = this.Halfedges[num];
				if (num2 >= 0)
				{
					list.Add(Delaunator.TriangleOfEdge(num2));
				}
			}
			return list;
		}

		// Token: 0x0600E89B RID: 59547 RVA: 0x00568435 File Offset: 0x00566635
		public static int NextHalfedge(int e)
		{
			if (e % 3 != 2)
			{
				return e + 1;
			}
			return e - 2;
		}

		// Token: 0x0600E89C RID: 59548 RVA: 0x00568444 File Offset: 0x00566644
		public static int PreviousHalfedge(int e)
		{
			if (e % 3 != 0)
			{
				return e - 1;
			}
			return e + 2;
		}

		// Token: 0x0600E89D RID: 59549 RVA: 0x00568452 File Offset: 0x00566652
		public static int[] EdgesOfTriangle(int t)
		{
			return new int[]
			{
				3 * t,
				3 * t + 1,
				3 * t + 2
			};
		}

		// Token: 0x0600E89E RID: 59550 RVA: 0x00568470 File Offset: 0x00566670
		public static int TriangleOfEdge(int e)
		{
			return e / 3;
		}

		// Token: 0x040093E2 RID: 37858
		private readonly double EPSILON = Math.Pow(2.0, -52.0);

		// Token: 0x040093E3 RID: 37859
		private readonly int[] EDGE_STACK = new int[512];

		// Token: 0x040093E8 RID: 37864
		private readonly int hashSize;

		// Token: 0x040093E9 RID: 37865
		private readonly int[] hullPrev;

		// Token: 0x040093EA RID: 37866
		private readonly int[] hullNext;

		// Token: 0x040093EB RID: 37867
		private readonly int[] hullTri;

		// Token: 0x040093EC RID: 37868
		private readonly int[] hullHash;

		// Token: 0x040093ED RID: 37869
		private double cx;

		// Token: 0x040093EE RID: 37870
		private double cy;

		// Token: 0x040093EF RID: 37871
		private int trianglesLen;

		// Token: 0x040093F0 RID: 37872
		private readonly double[] coords;

		// Token: 0x040093F1 RID: 37873
		private readonly int hullStart;

		// Token: 0x040093F2 RID: 37874
		private readonly int hullSize;
	}
}
