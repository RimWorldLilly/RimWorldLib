using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001202 RID: 4610
	public class Area_Allowed : Area
	{
		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x06007283 RID: 29315 RVA: 0x002817AC File Offset: 0x0027F9AC
		public override string Label
		{
			get
			{
				return this.labelInt;
			}
		}

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x06007284 RID: 29316 RVA: 0x002817B4 File Offset: 0x0027F9B4
		public override Color Color
		{
			get
			{
				return this.colorInt;
			}
		}

		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x06007285 RID: 29317 RVA: 0x002817BC File Offset: 0x0027F9BC
		public override bool Mutable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x06007286 RID: 29318 RVA: 0x002817BF File Offset: 0x0027F9BF
		public override int ListPriority
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x06007287 RID: 29319 RVA: 0x002817C6 File Offset: 0x0027F9C6
		// (set) Token: 0x06007288 RID: 29320 RVA: 0x002817D8 File Offset: 0x0027F9D8
		public override string RenamableLabel
		{
			get
			{
				return this.labelInt ?? this.BaseLabel;
			}
			set
			{
				this.labelInt = value;
			}
		}

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x06007289 RID: 29321 RVA: 0x002817E1 File Offset: 0x0027F9E1
		public override string BaseLabel
		{
			get
			{
				return "Area";
			}
		}

		// Token: 0x0600728A RID: 29322 RVA: 0x002817E8 File Offset: 0x0027F9E8
		public Area_Allowed()
		{
		}

		// Token: 0x0600728B RID: 29323 RVA: 0x002817FC File Offset: 0x0027F9FC
		public Area_Allowed(AreaManager areaManager, string label = null) : base(areaManager)
		{
			this.areaManager = areaManager;
			if (!label.NullOrEmpty())
			{
				this.labelInt = label;
			}
			else
			{
				int num = 1;
				for (;;)
				{
					this.labelInt = "AreaDefaultLabel".Translate(num);
					if (areaManager.GetLabeled(this.labelInt) == null)
					{
						break;
					}
					num++;
				}
			}
			this.colorInt = new Color(Rand.Value, Rand.Value, Rand.Value);
			this.colorInt = Color.Lerp(this.colorInt, Color.gray, 0.5f);
		}

		// Token: 0x0600728C RID: 29324 RVA: 0x0028189C File Offset: 0x0027FA9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.labelInt, "label", null, false);
			Scribe_Values.Look<Color>(ref this.colorInt, "color", default(Color), false);
		}

		// Token: 0x0600728D RID: 29325 RVA: 0x002818DB File Offset: 0x0027FADB
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x0600728E RID: 29326 RVA: 0x002818DE File Offset: 0x0027FADE
		public void SetColor(Color newColor)
		{
			this.colorInt = newColor;
			base.InvalidateColorTexture();
		}

		// Token: 0x0600728F RID: 29327 RVA: 0x002818ED File Offset: 0x0027FAED
		public void SetLabel(string newLabel)
		{
			this.labelInt = newLabel;
		}

		// Token: 0x06007290 RID: 29328 RVA: 0x002818F6 File Offset: 0x0027FAF6
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID.ToString() + "_Named_" + this.labelInt;
		}

		// Token: 0x06007291 RID: 29329 RVA: 0x00281918 File Offset: 0x0027FB18
		public override string ToString()
		{
			return this.labelInt;
		}

		// Token: 0x04004880 RID: 18560
		private string labelInt;

		// Token: 0x04004881 RID: 18561
		private Color colorInt = Color.red;
	}
}
