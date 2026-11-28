using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000543 RID: 1347
	public class CompLifespan : ThingComp
	{
		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x000F6B88 File Offset: 0x000F4D88
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000F6B95 File Offset: 0x000F4D95
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000F6BAF File Offset: 0x000F4DAF
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000F6BD8 File Offset: 0x000F4DD8
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000F6C08 File Offset: 0x000F4E08
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			string result = "";
			int num = this.Props.lifespanTicks - this.age;
			if (num > 0)
			{
				result = "LifespanExpiry".Translate() + " " + num.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
				if (!text.NullOrEmpty())
				{
					result = "\n" + text;
				}
			}
			return result;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000F6C84 File Offset: 0x000F4E84
		protected void Expire()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.Props.expireEffect != null)
			{
				this.Props.expireEffect.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			}
			if (this.Props.plantDefToSpawn != null && this.Props.plantDefToSpawn.CanNowPlantAt(this.parent.Position, this.parent.Map, false))
			{
				GenSpawn.Spawn(this.Props.plantDefToSpawn, this.parent.Position, this.parent.Map, WipeMode.Vanish);
			}
			this.parent.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x04001C52 RID: 7250
		public int age = -1;
	}
}
