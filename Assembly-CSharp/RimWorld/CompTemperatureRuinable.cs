using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B1 RID: 6577
	public class CompTemperatureRuinable : ThingComp
	{
		// Token: 0x17001CBA RID: 7354
		// (get) Token: 0x0600A514 RID: 42260 RVA: 0x003AEEA3 File Offset: 0x003AD0A3
		public CompProperties_TemperatureRuinable Props
		{
			get
			{
				return (CompProperties_TemperatureRuinable)this.props;
			}
		}

		// Token: 0x17001CBB RID: 7355
		// (get) Token: 0x0600A515 RID: 42261 RVA: 0x003AEEB0 File Offset: 0x003AD0B0
		public bool Ruined
		{
			get
			{
				return this.ruinedPercent >= 1f;
			}
		}

		// Token: 0x0600A516 RID: 42262 RVA: 0x003AEEC2 File Offset: 0x003AD0C2
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.ruinedPercent, "ruinedPercent", 0f, false);
		}

		// Token: 0x0600A517 RID: 42263 RVA: 0x003AEEDA File Offset: 0x003AD0DA
		public void Reset()
		{
			this.ruinedPercent = 0f;
		}

		// Token: 0x0600A518 RID: 42264 RVA: 0x003AEEE7 File Offset: 0x003AD0E7
		public override void CompTickRare()
		{
			this.CompTick();
		}

		// Token: 0x0600A519 RID: 42265 RVA: 0x003AEEF0 File Offset: 0x003AD0F0
		public override void CompTick()
		{
			if (this.Ruined)
			{
				return;
			}
			float ambientTemperature = this.parent.AmbientTemperature;
			if (ambientTemperature > this.Props.maxSafeTemperature)
			{
				this.ruinedPercent += (ambientTemperature - this.Props.maxSafeTemperature) * this.Props.progressPerDegreePerTick;
			}
			else if (ambientTemperature < this.Props.minSafeTemperature)
			{
				this.ruinedPercent -= (ambientTemperature - this.Props.minSafeTemperature) * this.Props.progressPerDegreePerTick;
			}
			if (this.ruinedPercent >= 1f)
			{
				this.ruinedPercent = 1f;
				this.parent.BroadcastCompSignal("RuinedByTemperature");
				return;
			}
			if (this.ruinedPercent < 0f)
			{
				this.ruinedPercent = 0f;
			}
		}

		// Token: 0x0600A51A RID: 42266 RVA: 0x003AEFC0 File Offset: 0x003AD1C0
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			CompTemperatureRuinable comp = ((ThingWithComps)otherStack).GetComp<CompTemperatureRuinable>();
			this.ruinedPercent = Mathf.Lerp(this.ruinedPercent, comp.ruinedPercent, t);
		}

		// Token: 0x0600A51B RID: 42267 RVA: 0x003AF004 File Offset: 0x003AD204
		public override bool AllowStackWith(Thing other)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)other).GetComp<CompTemperatureRuinable>();
			return this.Ruined == comp.Ruined;
		}

		// Token: 0x0600A51C RID: 42268 RVA: 0x003AF02B File Offset: 0x003AD22B
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompTemperatureRuinable>().ruinedPercent = this.ruinedPercent;
		}

		// Token: 0x0600A51D RID: 42269 RVA: 0x003AF044 File Offset: 0x003AD244
		public override string CompInspectStringExtra()
		{
			if (this.Ruined)
			{
				return "RuinedByTemperature".Translate();
			}
			if (this.ruinedPercent > 0f)
			{
				float ambientTemperature = this.parent.AmbientTemperature;
				string str;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					str = "Overheating".Translate();
				}
				else
				{
					if (ambientTemperature >= this.Props.minSafeTemperature)
					{
						return null;
					}
					str = "Freezing".Translate();
				}
				return str + ": " + this.ruinedPercent.ToStringPercent();
			}
			return null;
		}

		// Token: 0x04006186 RID: 24966
		protected float ruinedPercent;

		// Token: 0x04006187 RID: 24967
		public const string RuinedSignal = "RuinedByTemperature";
	}
}
