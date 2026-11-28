using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020A0 RID: 8352
	public struct ThingStuffPairWithQuality : IEquatable<ThingStuffPairWithQuality>, IExposable
	{
		// Token: 0x170025D3 RID: 9683
		// (get) Token: 0x0600D285 RID: 53893 RVA: 0x004D55A8 File Offset: 0x004D37A8
		public QualityCategory Quality
		{
			get
			{
				return this.quality.GetValueOrDefault(QualityCategory.Normal);
			}
		}

		// Token: 0x0600D286 RID: 53894 RVA: 0x004D55B8 File Offset: 0x004D37B8
		public ThingStuffPairWithQuality(ThingDef thing, ThingDef stuff, QualityCategory quality)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.quality = new QualityCategory?(quality);
			if (quality != QualityCategory.Normal && !thing.HasComp(typeof(CompQuality)))
			{
				Log.Warning(string.Concat(new string[]
				{
					"Created ThingStuffPairWithQuality with quality",
					quality.ToString(),
					" but ",
					(thing != null) ? thing.ToString() : null,
					" doesn't have CompQuality."
				}));
				quality = QualityCategory.Normal;
			}
			if (stuff != null && !thing.MadeFromStuff)
			{
				string[] array = new string[5];
				array[0] = "Created ThingStuffPairWithQuality with stuff ";
				int num = 1;
				ThingDef thingDef = stuff;
				array[num] = ((thingDef != null) ? thingDef.ToString() : null);
				array[2] = " but ";
				array[3] = ((thing != null) ? thing.ToString() : null);
				array[4] = " is not made from stuff.";
				Log.Warning(string.Concat(array));
				stuff = null;
			}
		}

		// Token: 0x0600D287 RID: 53895 RVA: 0x004D5696 File Offset: 0x004D3896
		public float GetStatValue(StatDef stat)
		{
			return stat.Worker.GetValue(StatRequest.For(this.thing, this.stuff, this.Quality), true);
		}

		// Token: 0x0600D288 RID: 53896 RVA: 0x004D56BC File Offset: 0x004D38BC
		public static bool operator ==(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			if (a.thing == b.thing && a.stuff == b.stuff)
			{
				QualityCategory? qualityCategory = a.quality;
				QualityCategory? qualityCategory2 = b.quality;
				return qualityCategory.GetValueOrDefault() == qualityCategory2.GetValueOrDefault() & qualityCategory != null == (qualityCategory2 != null);
			}
			return false;
		}

		// Token: 0x0600D289 RID: 53897 RVA: 0x004D5716 File Offset: 0x004D3916
		public static bool operator !=(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			return !(a == b);
		}

		// Token: 0x0600D28A RID: 53898 RVA: 0x004D5722 File Offset: 0x004D3922
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPairWithQuality && this.Equals((ThingStuffPairWithQuality)obj);
		}

		// Token: 0x0600D28B RID: 53899 RVA: 0x004D573A File Offset: 0x004D393A
		public bool Equals(ThingStuffPairWithQuality other)
		{
			return this == other;
		}

		// Token: 0x0600D28C RID: 53900 RVA: 0x004D5748 File Offset: 0x004D3948
		public override int GetHashCode()
		{
			return Gen.HashCombine<QualityCategory?>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.quality);
		}

		// Token: 0x0600D28D RID: 53901 RVA: 0x004D576C File Offset: 0x004D396C
		public static explicit operator ThingStuffPairWithQuality(ThingStuffPair p)
		{
			return new ThingStuffPairWithQuality(p.thing, p.stuff, QualityCategory.Normal);
		}

		// Token: 0x0600D28E RID: 53902 RVA: 0x004D5780 File Offset: 0x004D3980
		public Thing MakeThing(bool forceQuality = false)
		{
			Thing result = ThingMaker.MakeThing(this.thing, this.stuff);
			if (!forceQuality && result.HasComp<CompUniqueWeapon>())
			{
				return result;
			}
			CompQuality compQuality = result.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(this.Quality, new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			return result;
		}

		// Token: 0x0600D28F RID: 53903 RVA: 0x004D57CC File Offset: 0x004D39CC
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thing, "thing");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
		}

		// Token: 0x0400855B RID: 34139
		public ThingDef thing;

		// Token: 0x0400855C RID: 34140
		public ThingDef stuff;

		// Token: 0x0400855D RID: 34141
		public QualityCategory? quality;
	}
}
