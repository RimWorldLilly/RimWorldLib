using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001847 RID: 6215
	public class CompCascadeOnDestroyed : ThingComp
	{
		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x06009B98 RID: 39832 RVA: 0x00381612 File Offset: 0x0037F812
		private CompProperties_CascadeOnDestroyed Props
		{
			get
			{
				return (CompProperties_CascadeOnDestroyed)this.props;
			}
		}

		// Token: 0x06009B99 RID: 39833 RVA: 0x00381620 File Offset: 0x0037F820
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			if (dinfo != null && dinfo.Value.PreventCascade)
			{
				return;
			}
			int randomInRange = this.Props.cascadeCountRange.RandomInRange;
			int num = 0;
			Queue<Thing> queue = new Queue<Thing>();
			using (IEnumerator<Thing> enumerator = this.AdjacentCascadableThings(this.parent, prevMap).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Thing item = enumerator.Current;
					queue.Enqueue(item);
				}
				goto IL_10B;
			}
			IL_75:
			Thing thing = queue.Dequeue();
			foreach (Thing item2 in this.AdjacentCascadableThings(thing, prevMap))
			{
				if (!queue.Contains(item2))
				{
					queue.Enqueue(item2);
				}
			}
			thing.Kill(new DamageInfo?(new DamageInfo(null, 99999f, 0f, -1f, (dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, true)), null);
			num++;
			IL_10B:
			if (num >= randomInRange || queue.Empty<Thing>())
			{
				return;
			}
			goto IL_75;
		}

		// Token: 0x06009B9A RID: 39834 RVA: 0x00381764 File Offset: 0x0037F964
		private IEnumerable<Thing> AdjacentCascadableThings(Thing mass, Map map)
		{
			CompCascadeOnDestroyed.<AdjacentCascadableThings>d__3 <AdjacentCascadableThings>d__ = new CompCascadeOnDestroyed.<AdjacentCascadableThings>d__3(-2);
			<AdjacentCascadableThings>d__.<>4__this = this;
			<AdjacentCascadableThings>d__.<>3__mass = mass;
			<AdjacentCascadableThings>d__.<>3__map = map;
			return <AdjacentCascadableThings>d__;
		}
	}
}
