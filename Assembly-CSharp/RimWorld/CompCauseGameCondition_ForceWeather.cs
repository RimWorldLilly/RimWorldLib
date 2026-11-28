using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F8 RID: 6136
	public class CompCauseGameCondition_ForceWeather : CompCauseGameCondition
	{
		// Token: 0x0600992C RID: 39212 RVA: 0x00376FBF File Offset: 0x003751BF
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.weather = base.Props.conditionDef.weatherDef;
		}

		// Token: 0x0600992D RID: 39213 RVA: 0x00376FDE File Offset: 0x003751DE
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x0600992E RID: 39214 RVA: 0x00376FF6 File Offset: 0x003751F6
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompCauseGameCondition_ForceWeather.<CompGetGizmosExtra>d__3 <CompGetGizmosExtra>d__ = new CompCauseGameCondition_ForceWeather.<CompGetGizmosExtra>d__3(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600992F RID: 39215 RVA: 0x00377006 File Offset: 0x00375206
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_ForceWeather)condition).weather = this.weather;
		}

		// Token: 0x06009930 RID: 39216 RVA: 0x00377024 File Offset: 0x00375224
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "Weather".Translate() + ": " + this.weather.LabelCap;
		}

		// Token: 0x06009931 RID: 39217 RVA: 0x0037707B File Offset: 0x0037527B
		public override void RandomizeSettings(Site site)
		{
			this.weather = (from x in DefDatabase<WeatherDef>.AllDefsListForReading
			where x.isBad && x.canOccurAsRandomForcedEvent
			select x).RandomElement<WeatherDef>();
		}

		// Token: 0x04005B77 RID: 23415
		public WeatherDef weather;
	}
}
