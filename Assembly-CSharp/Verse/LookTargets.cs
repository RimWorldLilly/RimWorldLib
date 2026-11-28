using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020006A4 RID: 1700
	public class LookTargets : IExposable
	{
		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060035B9 RID: 13753 RVA: 0x0015BC1B File Offset: 0x00159E1B
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060035BA RID: 13754 RVA: 0x0015BC20 File Offset: 0x00159E20
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060035BB RID: 13755 RVA: 0x0015BC3B File Offset: 0x00159E3B
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060035BC RID: 13756 RVA: 0x0015BC4C File Offset: 0x00159E4C
		public GlobalTargetInfo PrimaryTarget
		{
			get
			{
				for (int i = 0; i < this.targets.Count; i++)
				{
					if (this.targets[i].IsValid)
					{
						return this.targets[i];
					}
				}
				if (this.targets.Count != 0)
				{
					return this.targets[0];
				}
				return GlobalTargetInfo.Invalid;
			}
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x0015BCB1 File Offset: 0x00159EB1
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, Array.Empty<object>());
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x0015BCC9 File Offset: 0x00159EC9
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x0015BCDC File Offset: 0x00159EDC
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x0015BD00 File Offset: 0x00159F00
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x0015BD24 File Offset: 0x00159F24
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x0015BD4A File Offset: 0x00159F4A
		public LookTargets(PlanetTile tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x0015BD6E File Offset: 0x00159F6E
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x0015BD90 File Offset: 0x00159F90
		public LookTargets(params GlobalTargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x0015BDD4 File Offset: 0x00159FD4
		public LookTargets(IEnumerable<TargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				IList<TargetInfo> list = targets as IList<TargetInfo>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						this.targets.Add(list[i]);
					}
					return;
				}
				foreach (TargetInfo target in targets)
				{
					this.targets.Add(target);
				}
			}
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x0015BE70 File Offset: 0x0015A070
		public LookTargets(params TargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x0015BEB6 File Offset: 0x0015A0B6
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x0015BED0 File Offset: 0x0015A0D0
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x0015BEEA File Offset: 0x0015A0EA
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x0015BF04 File Offset: 0x0015A104
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x0015BF1E File Offset: 0x0015A11E
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x0015BF38 File Offset: 0x0015A138
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x0015BF52 File Offset: 0x0015A152
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x0015BF6C File Offset: 0x0015A16C
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x0015BF74 File Offset: 0x0015A174
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x0015BF7C File Offset: 0x0015A17C
		public static implicit operator LookTargets(TargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x0015BF9F File Offset: 0x0015A19F
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x0015BFA7 File Offset: 0x0015A1A7
		public static implicit operator LookTargets(GlobalTargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x0015BFC5 File Offset: 0x0015A1C5
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x0015BFCD File Offset: 0x0015A1CD
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x0015BFD5 File Offset: 0x0015A1D5
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x0015BFDD File Offset: 0x0015A1DD
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x0015BFE5 File Offset: 0x0015A1E5
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x0015BFED File Offset: 0x0015A1ED
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x0015BFF5 File Offset: 0x0015A1F5
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x0015BFFD File Offset: 0x0015A1FD
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x0015C008 File Offset: 0x0015A208
		public static bool SameTargets(LookTargets a, LookTargets b)
		{
			if (a == null)
			{
				return b == null || !b.Any;
			}
			if (b == null)
			{
				return a == null || !a.Any;
			}
			if (a.targets.Count != b.targets.Count)
			{
				return false;
			}
			for (int i = 0; i < a.targets.Count; i++)
			{
				if (a.targets[i] != b.targets[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x0015C08C File Offset: 0x0015A28C
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x0015C0C4 File Offset: 0x0015A2C4
		private void AppendThingTargets<T>(IEnumerable<T> things) where T : Thing
		{
			if (things == null)
			{
				return;
			}
			IList<T> list = things as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in things)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x0015C158 File Offset: 0x0015A358
		private void AppendWorldObjectTargets<T>(IEnumerable<T> worldObjects) where T : WorldObject
		{
			if (worldObjects == null)
			{
				return;
			}
			IList<T> list = worldObjects as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in worldObjects)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x0015C1EC File Offset: 0x0015A3EC
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}

		// Token: 0x04002307 RID: 8967
		public List<GlobalTargetInfo> targets;
	}
}
