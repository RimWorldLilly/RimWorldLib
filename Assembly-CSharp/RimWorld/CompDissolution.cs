using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019DE RID: 6622
	public class CompDissolution : ThingComp
	{
		// Token: 0x17001D28 RID: 7464
		// (get) Token: 0x0600A6B7 RID: 42679 RVA: 0x003B6C39 File Offset: 0x003B4E39
		private CompProperties_Dissolution Props
		{
			get
			{
				return (CompProperties_Dissolution)this.props;
			}
		}

		// Token: 0x17001D29 RID: 7465
		// (get) Token: 0x0600A6B8 RID: 42680 RVA: 0x003B6C46 File Offset: 0x003B4E46
		public int DeterioarationRate
		{
			get
			{
				return (int)this.parent.GetStatValue(StatDefOf.DeteriorationRate, true, -1);
			}
		}

		// Token: 0x17001D2A RID: 7466
		// (get) Token: 0x0600A6B9 RID: 42681 RVA: 0x003B6C5B File Offset: 0x003B4E5B
		public int DissolutionAfterDamage
		{
			get
			{
				return this.DeterioarationRate * this.Props.dissolutionAfterDays;
			}
		}

		// Token: 0x17001D2B RID: 7467
		// (get) Token: 0x0600A6BA RID: 42682 RVA: 0x003B6C6F File Offset: 0x003B4E6F
		public int DefaultTicksUntilDissolution
		{
			get
			{
				return 60000 * this.Props.dissolutionAfterDays;
			}
		}

		// Token: 0x17001D2C RID: 7468
		// (get) Token: 0x0600A6BB RID: 42683 RVA: 0x003B6C84 File Offset: 0x003B4E84
		public bool IsOutdoors
		{
			get
			{
				Map mapHeld = this.parent.MapHeld;
				IntVec3 position = this.parent.Position;
				if (mapHeld != null)
				{
					Room room = position.GetRoom(mapHeld);
					if (room != null && room.UsesOutdoorTemperature)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001D2D RID: 7469
		// (get) Token: 0x0600A6BC RID: 42684 RVA: 0x003B6CC2 File Offset: 0x003B4EC2
		public bool IsFrozen
		{
			get
			{
				return this.parent.AmbientTemperature <= 0f;
			}
		}

		// Token: 0x17001D2E RID: 7470
		// (get) Token: 0x0600A6BD RID: 42685 RVA: 0x003B6CD9 File Offset: 0x003B4ED9
		public bool InAtomizer
		{
			get
			{
				return this.parent.ParentHolder != null && this.parent.ParentHolder is CompAtomizer;
			}
		}

		// Token: 0x17001D2F RID: 7471
		// (get) Token: 0x0600A6BE RID: 42686 RVA: 0x003B6CFD File Offset: 0x003B4EFD
		public bool CanDissolveNow
		{
			get
			{
				return !this.IsFrozen && !this.InAtomizer;
			}
		}

		// Token: 0x17001D30 RID: 7472
		// (get) Token: 0x0600A6BF RID: 42687 RVA: 0x003B6D12 File Offset: 0x003B4F12
		public bool IsBeingRainedOn
		{
			get
			{
				return this.IsOutdoors && this.parent.MapHeld.weatherManager.curWeather.rainRate > 0.1f;
			}
		}

		// Token: 0x17001D31 RID: 7473
		// (get) Token: 0x0600A6C0 RID: 42688 RVA: 0x003B6D40 File Offset: 0x003B4F40
		public int DissolutionIntervalTicks
		{
			get
			{
				float num = (float)this.DefaultTicksUntilDissolution;
				if (!this.IsOutdoors)
				{
					num /= this.Props.dissolutinFactorIndoors;
				}
				if (this.IsBeingRainedOn)
				{
					num /= this.Props.dissolutionFactorRain;
				}
				return (int)num;
			}
		}

		// Token: 0x0600A6C1 RID: 42689 RVA: 0x003B6D83 File Offset: 0x003B4F83
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Dissolution"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x0600A6C2 RID: 42690 RVA: 0x003B6DA8 File Offset: 0x003B4FA8
		public void TriggerDissolutionEvent(int amountToDissolve = 1)
		{
			amountToDissolve = Mathf.Min(amountToDissolve, this.parent.stackCount);
			if (this.parent.MapHeld != null)
			{
				this.DissolveMap(amountToDissolve);
			}
			else
			{
				this.DissolveWorld(amountToDissolve, this.parent.Tile);
			}
			this.parent.stackCount -= amountToDissolve;
			this.dissolveTicks = 0;
			if (this.parent.stackCount <= 0 && !this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600A6C3 RID: 42691 RVA: 0x003B6E34 File Offset: 0x003B5034
		private void DissolveMap(int amount)
		{
			List<ThingComp> allComps = this.parent.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				CompDissolutionEffect compDissolutionEffect = allComps[i] as CompDissolutionEffect;
				if (compDissolutionEffect != null)
				{
					compDissolutionEffect.DoDissolutionEffectMap(amount);
				}
			}
			Find.Storyteller.Notify_DissolutionEvent(this.parent);
		}

		// Token: 0x0600A6C4 RID: 42692 RVA: 0x003B6E88 File Offset: 0x003B5088
		private void DissolveWorld(int amount, PlanetTile tile)
		{
			List<ThingComp> allComps = this.parent.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				CompDissolutionEffect compDissolutionEffect = allComps[i] as CompDissolutionEffect;
				if (compDissolutionEffect != null)
				{
					compDissolutionEffect.DoDissolutionEffectWorld(amount, tile);
				}
			}
		}

		// Token: 0x0600A6C5 RID: 42693 RVA: 0x003B6ECA File Offset: 0x003B50CA
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(ref dinfo, out absorbed);
			if (dinfo.Def.harmsHealth && dinfo.Amount >= (float)this.parent.HitPoints)
			{
				this.DissolveMap(this.parent.stackCount);
			}
		}

		// Token: 0x0600A6C6 RID: 42694 RVA: 0x003B6F06 File Offset: 0x003B5106
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			this.DissolveWorld(this.parent.stackCount, tile);
		}

		// Token: 0x0600A6C7 RID: 42695 RVA: 0x003B6F1A File Offset: 0x003B511A
		public override void CompTick()
		{
			base.CompTick();
			if (!this.CanDissolveNow)
			{
				return;
			}
			this.dissolveTicks++;
			if (this.dissolveTicks >= this.DissolutionIntervalTicks)
			{
				this.TriggerDissolutionEvent(1);
			}
		}

		// Token: 0x0600A6C8 RID: 42696 RVA: 0x003B6F4E File Offset: 0x003B514E
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompDissolution.<CompGetGizmosExtra>d__28 <CompGetGizmosExtra>d__ = new CompDissolution.<CompGetGizmosExtra>d__28(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A6C9 RID: 42697 RVA: 0x003B6F60 File Offset: 0x003B5160
		public override string CompInspectStringExtra()
		{
			if (this.InAtomizer)
			{
				return base.CompInspectStringExtra();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.IsFrozen)
			{
				stringBuilder.Append("DissolutionFrozen".Translate());
				return stringBuilder.ToString();
			}
			CompDissolution.tmpDissolutionReasons.Clear();
			if (!this.IsOutdoors)
			{
				CompDissolution.tmpDissolutionReasons.Add("DissolutionRateIndoors".Translate() + " x" + this.Props.dissolutinFactorIndoors.ToStringPercent());
			}
			if (this.IsBeingRainedOn)
			{
				CompDissolution.tmpDissolutionReasons.Add("DissolutionRain".Translate() + " x" + this.Props.dissolutionFactorRain.ToStringPercent());
			}
			stringBuilder.Append("DissolvesEvery".Translate(this.DissolutionIntervalTicks.ToStringTicksToPeriod(true, false, true, true, false)));
			if (CompDissolution.tmpDissolutionReasons.Count > 0)
			{
				string str = CompDissolution.tmpDissolutionReasons.ToLineList(null);
				stringBuilder.Append(" (" + str.CapitalizeFirst() + ")");
				CompDissolution.tmpDissolutionReasons.Clear();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A6CA RID: 42698 RVA: 0x003B70BE File Offset: 0x003B52BE
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.dissolveTicks, "dissolveTicks", 0, false);
		}

		// Token: 0x04006244 RID: 25156
		private int dissolveTicks;

		// Token: 0x04006245 RID: 25157
		private static List<string> tmpDissolutionReasons = new List<string>();
	}
}
