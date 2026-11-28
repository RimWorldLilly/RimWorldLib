using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005A6 RID: 1446
	public class ThingWithComps : Thing
	{
		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x001082D2 File Offset: 0x001064D2
		public List<ThingComp> AllComps
		{
			get
			{
				if (this.comps == null)
				{
					return ThingWithComps.EmptyCompsList;
				}
				return this.comps;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06002B76 RID: 11126 RVA: 0x001082E8 File Offset: 0x001064E8
		// (set) Token: 0x06002B77 RID: 11127 RVA: 0x00108370 File Offset: 0x00106570
		public override Color DrawColor
		{
			get
			{
				CompColorable comp = this.GetComp<CompColorable>();
				if (comp != null && comp.Active)
				{
					return comp.Color;
				}
				foreach (ThingComp thingComp in this.AllComps)
				{
					Color? color = thingComp.ForceColor();
					if (color != null)
					{
						return color.Value;
					}
				}
				return base.DrawColor;
			}
			set
			{
				this.SetColor(value, true);
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06002B78 RID: 11128 RVA: 0x0010837C File Offset: 0x0010657C
		public override string LabelNoCount
		{
			get
			{
				string text = base.LabelNoCount;
				if (this.comps != null)
				{
					int i = 0;
					int count = this.comps.Count;
					while (i < count)
					{
						text = this.comps[i].TransformLabel(text);
						i++;
					}
				}
				return text;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06002B79 RID: 11129 RVA: 0x001083C4 File Offset: 0x001065C4
		public override string DescriptionFlavor
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.DescriptionFlavor);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string descriptionPart = this.comps[i].GetDescriptionPart();
						if (!descriptionPart.NullOrEmpty())
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.AppendLine();
								stringBuilder.AppendLine();
							}
							stringBuilder.Append(descriptionPart);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x00108444 File Offset: 0x00106644
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostMake();
				}
			}
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x0010848C File Offset: 0x0010668C
		public T GetComp<T>() where T : ThingComp
		{
			if (this.comps == null)
			{
				return default(T);
			}
			int count = this.comps.Count;
			if (count >= 3)
			{
				if (this.compsByType != null)
				{
					if (count != this.compsByTypeCount || this.comps[count - 1] != this.compsByTypeLast)
					{
						this.RebuildCompsByType();
					}
					ThingComp[] array;
					if (this.compsByType.TryGetValue(typeof(T), out array))
					{
						return (T)((object)array[0]);
					}
					if (typeof(T).IsSealedWithCache())
					{
						return default(T);
					}
				}
				for (int i = 0; i < count; i++)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
				return default(T);
			}
			T t2 = this.comps[0] as T;
			if (t2 != null)
			{
				return t2;
			}
			if (count == 2)
			{
				T t3 = this.comps[1] as T;
				if (t3 != null)
				{
					return t3;
				}
			}
			return default(T);
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x001085B4 File Offset: 0x001067B4
		public IEnumerable<T> GetComps<T>() where T : class
		{
			ThingWithComps.<GetComps>d__19<T> <GetComps>d__ = new ThingWithComps.<GetComps>d__19<T>(-2);
			<GetComps>d__.<>4__this = this;
			return <GetComps>d__;
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x001085C4 File Offset: 0x001067C4
		public ThingComp GetCompByDefType(CompProperties def)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].props.compClass == def.compClass)
					{
						return this.comps[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x00108620 File Offset: 0x00106820
		public void InitializeComps()
		{
			if (this.def.comps.Any<CompProperties>())
			{
				this.comps = new List<ThingComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					ThingComp thingComp = null;
					try
					{
						thingComp = (ThingComp)Activator.CreateInstance(this.def.comps[i].compClass);
						thingComp.parent = this;
						this.comps.Add(thingComp);
						thingComp.Initialize(this.def.comps[i]);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate or initialize a ThingComp: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
						this.comps.Remove(thingComp);
					}
				}
				this.RebuildCompsByType();
				this.compStyleable = this.GetComp<CompStyleable>();
				this.compForbiddable = this.GetComp<CompForbiddable>();
				this.compQuality = this.GetComp<CompQuality>();
			}
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x00108720 File Offset: 0x00106920
		private void RebuildCompsByType()
		{
			int count = this.comps.Count;
			Dictionary<Type, List<ThingComp>> dictionary = new Dictionary<Type, List<ThingComp>>();
			for (int i = 0; i < count; i++)
			{
				List<ThingComp> list;
				if (!dictionary.TryGetValue(this.comps[i].GetType(), out list))
				{
					list = (dictionary[this.comps[i].GetType()] = new List<ThingComp>());
				}
				list.Add(this.comps[i]);
			}
			Dictionary<Type, ThingComp[]> dictionary2 = new Dictionary<Type, ThingComp[]>(dictionary.Count);
			foreach (KeyValuePair<Type, List<ThingComp>> keyValuePair in dictionary)
			{
				dictionary2[keyValuePair.Key] = keyValuePair.Value.ToArray();
			}
			this.compsByType = dictionary2;
			this.compsByTypeCount = count;
			this.compsByTypeLast = ((count > 0) ? this.comps[count - 1] : null);
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x00108824 File Offset: 0x00106A24
		public override string GetCustomLabelNoCount(bool includeHp = true)
		{
			string text = base.GetCustomLabelNoCount(includeHp);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					text = this.comps[i].TransformLabel(text);
					i++;
				}
			}
			return text;
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x00108870 File Offset: 0x00106A70
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostExposeData();
				}
			}
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x001088C0 File Offset: 0x00106AC0
		public void BroadcastCompSignal(string signal)
		{
			this.ReceiveCompSignal(signal);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].ReceiveCompSignal(signal);
					i++;
				}
			}
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x00108906 File Offset: 0x00106B06
		protected virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x00108908 File Offset: 0x00106B08
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSpawnSetup(respawningAfterLoad);
				}
			}
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x00108950 File Offset: 0x00106B50
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDeSpawn(map, mode);
				}
			}
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x0010899C File Offset: 0x00106B9C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map mapHeld = base.MapHeld;
			base.Destroy(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDestroy(mode, mapHeld);
				}
			}
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x001089E8 File Offset: 0x00106BE8
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			Pawn pawn = this as Pawn;
			Map prevMap;
			if (pawn != null)
			{
				prevMap = (pawn.prevMap ?? base.MapHeld);
			}
			else
			{
				prevMap = base.MapHeld;
			}
			base.Kill(dinfo, exactCulprit);
			if (this.comps == null)
			{
				return;
			}
			foreach (ThingComp thingComp in this.comps)
			{
				thingComp.Notify_Killed(prevMap, dinfo);
			}
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x00108A70 File Offset: 0x00106C70
		protected override void Tick()
		{
			if (this.comps == null)
			{
				return;
			}
			int i = 0;
			int count = this.comps.Count;
			while (i < count)
			{
				this.comps[i].CompTick();
				i++;
			}
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x00108AB0 File Offset: 0x00106CB0
		protected override void TickInterval(int delta)
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickInterval(delta);
					i++;
				}
			}
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x00108AF0 File Offset: 0x00106CF0
		public override void TickRare()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickRare();
					i++;
				}
			}
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x00108B30 File Offset: 0x00106D30
		public override void TickLong()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickLong();
					i++;
				}
			}
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x00108B70 File Offset: 0x00106D70
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPreApplyDamage(ref dinfo, out absorbed);
					if (absorbed)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x00108BC0 File Offset: 0x00106DC0
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostApplyDamage(dinfo, totalDamageDealt);
				}
			}
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x00108C06 File Offset: 0x00106E06
		public virtual IEnumerable<ThingDefCountClass> GetAdditionalLeavings(DestroyMode mode)
		{
			ThingWithComps.<GetAdditionalLeavings>d__37 <GetAdditionalLeavings>d__ = new ThingWithComps.<GetAdditionalLeavings>d__37(-2);
			<GetAdditionalLeavings>d__.<>4__this = this;
			<GetAdditionalLeavings>d__.<>3__mode = mode;
			return <GetAdditionalLeavings>d__;
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x00108C1D File Offset: 0x00106E1D
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (!this.CompsPreventDrawing())
			{
				base.DrawAt(drawLoc, flip);
			}
			this.Comps_DrawAt(drawLoc, flip);
			this.Comps_PostDraw();
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x00108C40 File Offset: 0x00106E40
		protected void Comps_DrawAt(Vector3 drawLoc, bool flip)
		{
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].DrawAt(drawLoc, flip);
			}
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x00108C80 File Offset: 0x00106E80
		protected void Comps_PostDraw()
		{
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDraw();
			}
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x00108CC0 File Offset: 0x00106EC0
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDrawExtraSelectionOverlays();
			}
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x00108D04 File Offset: 0x00106F04
		public override void Print(SectionLayer layer)
		{
			if (!this.CompsPreventDrawing())
			{
				base.Print(layer);
			}
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostPrintOnto(layer);
			}
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x00108D54 File Offset: 0x00106F54
		private bool CompsPreventDrawing()
		{
			if (this.comps == null)
			{
				return false;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].DontDrawParent())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x00108D98 File Offset: 0x00106F98
		public virtual void PrintForPowerGrid(SectionLayer layer)
		{
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPrintForPowerGrid(layer);
			}
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x00108DD6 File Offset: 0x00106FD6
		public override IEnumerable<Gizmo> GetGizmos()
		{
			ThingWithComps.<GetGizmos>d__45 <GetGizmos>d__ = new ThingWithComps.<GetGizmos>d__45(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x00108DE8 File Offset: 0x00106FE8
		public override bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			if (!this.CanStackWith(other))
			{
				return false;
			}
			int count = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PreAbsorbStack(other, count);
				}
			}
			return base.TryAbsorbStack(other, respectStackLimit);
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x00108E44 File Offset: 0x00107044
		public override Thing SplitOff(int count)
		{
			Thing thing = base.SplitOff(count);
			if (thing != null && this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSplitOff(thing);
				}
			}
			return thing;
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x00108E90 File Offset: 0x00107090
		public override bool CanStackWith(Thing other)
		{
			if (!base.CanStackWith(other))
			{
				return false;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (!this.comps[i].AllowStackWith(other))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x00108EE0 File Offset: 0x001070E0
		public override TipSignal GetTooltip()
		{
			string text = base.LabelNoParenthesisCap.AsTipTitle() + GenLabel.LabelExtras(this, true, true);
			text = text + "\n\n" + this.DescriptionDetailed;
			if (this.def.useHitPoints)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					this.HitPoints.ToString(),
					" / ",
					base.MaxHitPoints.ToString()
				});
			}
			text += "\n";
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					string text2 = this.comps[i].CompTipStringExtra();
					if (!text2.NullOrEmpty())
					{
						text = text + "\n" + text2;
					}
				}
			}
			return new TipSignal(text, this.thingIDNumber * 251235);
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x00108FCC File Offset: 0x001071CC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			string text = this.InspectStringPartsFromComps();
			if (!text.NullOrEmpty())
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x0010901C File Offset: 0x0010721C
		protected string InspectStringPartsFromComps()
		{
			if (this.comps == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
					{
						Type type = this.comps[i].GetType();
						Log.ErrorOnce(((type != null) ? type.ToString() : null) + " CompInspectStringExtra ended with whitespace: " + text, 25612);
						text = text.TrimEndNewlines();
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x001090E4 File Offset: 0x001072E4
		public override void DrawGUIOverlay()
		{
			base.DrawGUIOverlay();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].DrawGUIOverlay();
				}
			}
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x00109126 File Offset: 0x00107326
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			ThingWithComps.<GetFloatMenuOptions>d__54 <GetFloatMenuOptions>d__ = new ThingWithComps.<GetFloatMenuOptions>d__54(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x0010913D File Offset: 0x0010733D
		public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			ThingWithComps.<GetMultiSelectFloatMenuOptions>d__55 <GetMultiSelectFloatMenuOptions>d__ = new ThingWithComps.<GetMultiSelectFloatMenuOptions>d__55(-2);
			<GetMultiSelectFloatMenuOptions>d__.<>4__this = this;
			<GetMultiSelectFloatMenuOptions>d__.<>3__selPawns = selPawns;
			return <GetMultiSelectFloatMenuOptions>d__;
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x00109154 File Offset: 0x00107354
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PrePreTraded(action, playerNegotiator, trader);
				}
			}
			base.PreTraded(action, playerNegotiator, trader);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x0010919C File Offset: 0x0010739C
		public override void PostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			base.PostGeneratedForTrader(trader, forTile, forFaction);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostGeneratedForTrader(trader, forTile, forFaction);
				}
			}
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x001091E4 File Offset: 0x001073E4
		protected override void PrePostIngested(Pawn ingester)
		{
			base.PrePostIngested(ingester);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PrePostIngested(ingester);
				}
			}
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x00109228 File Offset: 0x00107428
		protected override void PostIngested(Pawn ingester)
		{
			base.PostIngested(ingester);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostIngested(ingester);
				}
			}
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x0010926C File Offset: 0x0010746C
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_DefsHotReloaded();
				}
			}
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x001092B0 File Offset: 0x001074B0
		public override void PostMapInit()
		{
			base.PostMapInit();
			if (this.comps == null)
			{
				return;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostMapInit();
			}
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x001092F4 File Offset: 0x001074F4
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_SignalReceived(signal);
				}
			}
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x00109338 File Offset: 0x00107538
		public override void Notify_RecipeProduced(Pawn pawn)
		{
			base.Notify_RecipeProduced(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_RecipeProduced(pawn);
				}
			}
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x0010937C File Offset: 0x0010757C
		public override void Notify_LordDestroyed()
		{
			base.Notify_LordDestroyed();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_LordDestroyed();
				}
			}
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x001093C0 File Offset: 0x001075C0
		public override void Notify_Equipped(Pawn pawn)
		{
			base.Notify_Equipped(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_Equipped(pawn);
				}
			}
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x00109404 File Offset: 0x00107604
		public override void Notify_Unequipped(Pawn pawn)
		{
			base.Notify_Unequipped(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_Unequipped(pawn);
				}
			}
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x00109448 File Offset: 0x00107648
		public override void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
			base.Notify_UsedVerb(pawn, verb);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_UsedVerb(pawn, verb);
				}
			}
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x00109490 File Offset: 0x00107690
		public override void Notify_UsedWeapon(Pawn pawn)
		{
			base.Notify_UsedWeapon(pawn);
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				IdeoWeaponDisposition dispositionForWeapon = pawn.Ideo.GetDispositionForWeapon(this.def);
				if (dispositionForWeapon == IdeoWeaponDisposition.Despised)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedDespisedWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
				else if (dispositionForWeapon == IdeoWeaponDisposition.Noble)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedNobleWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_UsedWeapon(pawn);
				}
			}
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x00109540 File Offset: 0x00107740
		public void Notify_KilledPawn(Pawn pawn)
		{
			if (ModsConfig.IdeologyActive && pawn.Ideo != null && this.def.IsWeapon)
			{
				IdeoWeaponDisposition dispositionForWeapon = pawn.Ideo.GetDispositionForWeapon(this.def);
				if (dispositionForWeapon == IdeoWeaponDisposition.Despised)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KillWithDespisedWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
				else if (dispositionForWeapon == IdeoWeaponDisposition.Noble)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KillWithNobleWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_KilledPawn(pawn);
				}
			}
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x001095F8 File Offset: 0x001077F8
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			base.Notify_AbandonedAtTile(tile);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_AbandonedAtTile(tile);
				}
			}
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x0010963C File Offset: 0x0010783C
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			base.Notify_KilledLeavingsLeft(leavings);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_KilledLeavingsLeft(leavings);
				}
			}
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x00109680 File Offset: 0x00107880
		public override void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
		{
			List<ThingComp> allComps = this.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				IThingStudied thingStudied = allComps[i] as IThingStudied;
				if (thingStudied != null)
				{
					thingStudied.OnStudied(studier, amount, category);
				}
			}
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x001096C0 File Offset: 0x001078C0
		public override void Notify_ColorChanged()
		{
			base.Notify_ColorChanged();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_ColorChanged();
				}
			}
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x00109702 File Offset: 0x00107902
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			ThingWithComps.<SpecialDisplayStats>d__74 <SpecialDisplayStats>d__ = new ThingWithComps.<SpecialDisplayStats>d__74(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x00109714 File Offset: 0x00107914
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			CompWakeUpDormant comp = this.GetComp<CompWakeUpDormant>();
			if (comp != null && (explosion.Position - base.Position).LengthHorizontal <= explosion.radius)
			{
				comp.Activate(explosion.instigator, true, false, false);
			}
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x00109764 File Offset: 0x00107964
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			if (this.def.notifyMapRemoved && this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_MapRemoved();
				}
			}
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x001097B4 File Offset: 0x001079B4
		public override void PreSwapMap()
		{
			base.PreSwapMap();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PreSwapMap();
				}
			}
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x001097F8 File Offset: 0x001079F8
		public override void PostSwapMap()
		{
			base.PostSwapMap();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSwapMap();
				}
			}
		}

		// Token: 0x04001D83 RID: 7555
		private List<ThingComp> comps;

		// Token: 0x04001D84 RID: 7556
		private Dictionary<Type, ThingComp[]> compsByType;

		// Token: 0x04001D85 RID: 7557
		private int compsByTypeCount;

		// Token: 0x04001D86 RID: 7558
		private ThingComp compsByTypeLast;

		// Token: 0x04001D87 RID: 7559
		[Unsaved(false)]
		public CompStyleable compStyleable;

		// Token: 0x04001D88 RID: 7560
		[Unsaved(false)]
		public CompForbiddable compForbiddable;

		// Token: 0x04001D89 RID: 7561
		[Unsaved(false)]
		public CompQuality compQuality;

		// Token: 0x04001D8A RID: 7562
		private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();

		// Token: 0x04001D8B RID: 7563
		private static List<FloatMenuOption> tmpFloatMenuOptions = new List<FloatMenuOption>();
	}
}
