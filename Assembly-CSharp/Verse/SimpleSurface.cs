using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006AB RID: 1707
	public class SimpleSurface : IEnumerable<SurfaceColumn>, IEnumerable
	{
		// Token: 0x0600361F RID: 13855 RVA: 0x0015D604 File Offset: 0x0015B804
		public float Evaluate(float x, float y)
		{
			if (this.columns.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve2D with no columns.");
				return 0f;
			}
			if (x <= this.columns[0].x)
			{
				return this.columns[0].y.Evaluate(y);
			}
			if (x >= this.columns[this.columns.Count - 1].x)
			{
				return this.columns[this.columns.Count - 1].y.Evaluate(y);
			}
			SurfaceColumn surfaceColumn = this.columns[0];
			SurfaceColumn surfaceColumn2 = this.columns[this.columns.Count - 1];
			int i = 0;
			while (i < this.columns.Count)
			{
				if (x <= this.columns[i].x)
				{
					surfaceColumn2 = this.columns[i];
					if (i > 0)
					{
						surfaceColumn = this.columns[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - surfaceColumn.x) / (surfaceColumn2.x - surfaceColumn.x);
			return Mathf.Lerp(surfaceColumn.y.Evaluate(y), surfaceColumn2.y.Evaluate(y), t);
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x0015D745 File Offset: 0x0015B945
		public void Add(SurfaceColumn newColumn)
		{
			this.columns.Add(newColumn);
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x0015D753 File Offset: 0x0015B953
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x0015D75B File Offset: 0x0015B95B
		public IEnumerator<SurfaceColumn> GetEnumerator()
		{
			SimpleSurface.<GetEnumerator>d__4 <GetEnumerator>d__ = new SimpleSurface.<GetEnumerator>d__4(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x0015D76A File Offset: 0x0015B96A
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			SimpleSurface.<ConfigErrors>d__5 <ConfigErrors>d__ = new SimpleSurface.<ConfigErrors>d__5(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__prefix = prefix;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400232D RID: 9005
		private List<SurfaceColumn> columns = new List<SurfaceColumn>();
	}
}
