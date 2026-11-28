using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006A6 RID: 1702
	public struct CurveMark
	{
		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060035E3 RID: 13795 RVA: 0x0015C24B File Offset: 0x0015A44B
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060035E4 RID: 13796 RVA: 0x0015C253 File Offset: 0x0015A453
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060035E5 RID: 13797 RVA: 0x0015C25B File Offset: 0x0015A45B
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x0015C263 File Offset: 0x0015A463
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x04002308 RID: 8968
		private float x;

		// Token: 0x04002309 RID: 8969
		private string message;

		// Token: 0x0400230A RID: 8970
		private Color color;
	}
}
