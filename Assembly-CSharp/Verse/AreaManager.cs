using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200023D RID: 573
	public class AreaManager : IExposable
	{
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x00060A69 File Offset: 0x0005EC69
		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060010E8 RID: 4328 RVA: 0x00060A71 File Offset: 0x0005EC71
		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x00060A79 File Offset: 0x0005EC79
		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x00060A81 File Offset: 0x0005EC81
		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x00060A89 File Offset: 0x0005EC89
		public Area_SnowOrSandClear SnowOrSandClear
		{
			get
			{
				return this.Get<Area_SnowOrSandClear>();
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x00060A91 File Offset: 0x0005EC91
		public Area_PollutionClear PollutionClear
		{
			get
			{
				return this.Get<Area_PollutionClear>();
			}
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00060A99 File Offset: 0x0005EC99
		public AreaManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00060AB4 File Offset: 0x0005ECB4
		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowOrSandClear(this));
			if (ModsConfig.BiotechActive)
			{
				this.areas.Add(new Area_PollutionClear(this));
			}
			if (!this.map.wasSpawnedViaGravShipLanding)
			{
				Area_Allowed area_Allowed;
				this.TryMakeNewAllowed(out area_Allowed);
			}
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00060B34 File Offset: 0x0005ED34
		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && ModsConfig.BiotechActive && this.PollutionClear == null)
			{
				Area_PollutionClear area_PollutionClear = new Area_PollutionClear(this);
				area_PollutionClear.areaManager = this;
				this.areas.Add(area_PollutionClear);
			}
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00060B98 File Offset: 0x0005ED98
		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00060BCC File Offset: 0x0005EDCC
		internal void Remove(Area area)
		{
			if (!area.Mutable)
			{
				Log.Error("Tried to delete non-Deletable area " + ((area != null) ? area.ToString() : null));
				return;
			}
			this.areas.Remove(area);
			this.NotifyEveryoneAreaRemoved(area);
			if (Designator_AreaAllowed.SelectedArea == area)
			{
				Designator_AreaAllowed.ClearSelectedArea();
			}
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00060C20 File Offset: 0x0005EE20
		public Area GetLabeled(string s)
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				if (this.areas[i].Label == s)
				{
					return this.areas[i];
				}
			}
			return null;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00060C6C File Offset: 0x0005EE6C
		public T Get<T>() where T : Area
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				T t = this.areas[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00060CB9 File Offset: 0x0005EEB9
		private void SortAreas()
		{
			this.areas.InsertionSort((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority));
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00060CE8 File Offset: 0x0005EEE8
		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00060D20 File Offset: 0x0005EF20
		private void NotifyEveryoneAreaRemoved(Area area)
		{
			foreach (Pawn pawn in PawnsFinder.All_AliveOrDead)
			{
				if (pawn.playerSettings != null)
				{
					pawn.playerSettings.Notify_AreaRemoved(area);
				}
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00060D80 File Offset: 0x0005EF80
		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00060DB5 File Offset: 0x0005EFB5
		public bool CanMakeNewAllowed()
		{
			return (from a in this.areas
			where a is Area_Allowed
			select a).Count<Area>() < 10;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00060DEA File Offset: 0x0005EFEA
		public bool TryMakeNewAllowed(out Area_Allowed area)
		{
			if (!this.CanMakeNewAllowed())
			{
				area = null;
				return false;
			}
			area = new Area_Allowed(this, null);
			this.areas.Add(area);
			this.SortAreas();
			return true;
		}

		// Token: 0x04000F83 RID: 3971
		public Map map;

		// Token: 0x04000F84 RID: 3972
		private List<Area> areas = new List<Area>();

		// Token: 0x04000F85 RID: 3973
		public const int MaxAllowedAreas = 10;
	}
}
