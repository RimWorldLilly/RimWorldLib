using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000546 RID: 1350
	public abstract class ThingComp
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060027A1 RID: 10145 RVA: 0x000F6E12 File Offset: 0x000F5012
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000F6E1F File Offset: 0x000F501F
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000F6E28 File Offset: 0x000F5028
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000F6E2A File Offset: 0x000F502A
		public virtual void PostExposeData()
		{
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x000F6E2C File Offset: 0x000F502C
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x000F6E2E File Offset: 0x000F502E
		public virtual void PostMapInit()
		{
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000F6E30 File Offset: 0x000F5030
		public virtual void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000F6E32 File Offset: 0x000F5032
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000F6E34 File Offset: 0x000F5034
		public virtual void PostPostMake()
		{
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000F6E36 File Offset: 0x000F5036
		public virtual void CompTick()
		{
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000F6E38 File Offset: 0x000F5038
		public virtual void CompTickInterval(int delta)
		{
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000F6E3A File Offset: 0x000F503A
		public virtual void CompTickRare()
		{
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000F6E3C File Offset: 0x000F503C
		public virtual void CompTickLong()
		{
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x000F6E3E File Offset: 0x000F503E
		public virtual void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000F6E43 File Offset: 0x000F5043
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000F6E45 File Offset: 0x000F5045
		public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000F6E47 File Offset: 0x000F5047
		public virtual void PostDraw()
		{
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000F6E49 File Offset: 0x000F5049
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000F6E4B File Offset: 0x000F504B
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000F6E4D File Offset: 0x000F504D
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000F6E4F File Offset: 0x000F504F
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000F6E51 File Offset: 0x000F5051
		public virtual void PostSplitOff(Thing piece)
		{
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000F6E53 File Offset: 0x000F5053
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000F6E56 File Offset: 0x000F5056
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000F6E5D File Offset: 0x000F505D
		public virtual IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000F6E64 File Offset: 0x000F5064
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000F6E67 File Offset: 0x000F5067
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x000F6E6A File Offset: 0x000F506A
		public virtual string CompTipStringExtra()
		{
			return null;
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000F6E6D File Offset: 0x000F506D
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000F6E70 File Offset: 0x000F5070
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000F6E77 File Offset: 0x000F5077
		public virtual IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000F6E7E File Offset: 0x000F507E
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000F6E80 File Offset: 0x000F5080
		public virtual void PrePostIngested(Pawn ingester)
		{
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000F6E82 File Offset: 0x000F5082
		public virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000F6E84 File Offset: 0x000F5084
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000F6E86 File Offset: 0x000F5086
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000F6E88 File Offset: 0x000F5088
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x000F6E8A File Offset: 0x000F508A
		public virtual void Notify_MapRemoved()
		{
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x000F6E8C File Offset: 0x000F508C
		public virtual void DrawGUIOverlay()
		{
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x000F6E8E File Offset: 0x000F508E
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return null;
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x000F6E91 File Offset: 0x000F5091
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x000F6E93 File Offset: 0x000F5093
		public virtual void Notify_Unequipped(Pawn pawn)
		{
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x000F6E95 File Offset: 0x000F5095
		public virtual void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x000F6E97 File Offset: 0x000F5097
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x000F6E99 File Offset: 0x000F5099
		public virtual void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000F6E9B File Offset: 0x000F509B
		public virtual void Notify_KilledPawn(Pawn pawn)
		{
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x000F6E9D File Offset: 0x000F509D
		public virtual void Notify_PassedToWorld()
		{
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000F6E9F File Offset: 0x000F509F
		public virtual void Notify_WearerDied()
		{
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000F6EA1 File Offset: 0x000F50A1
		public virtual void Notify_Downed()
		{
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000F6EA3 File Offset: 0x000F50A3
		public virtual void Notify_Released()
		{
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x000F6EA5 File Offset: 0x000F50A5
		public virtual void Notify_DefsHotReloaded()
		{
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x000F6EA7 File Offset: 0x000F50A7
		public virtual void Notify_AddBedThoughts(Pawn pawn)
		{
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x000F6EA9 File Offset: 0x000F50A9
		public virtual void Notify_AbandonedAtTile(PlanetTile tile)
		{
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x000F6EAB File Offset: 0x000F50AB
		public virtual void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x000F6EAD File Offset: 0x000F50AD
		public virtual void Notify_Arrested(bool succeeded)
		{
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000F6EAF File Offset: 0x000F50AF
		public virtual void Notify_PrisonBreakout()
		{
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x000F6EB1 File Offset: 0x000F50B1
		public virtual void Notify_Hacked(Pawn hacker = null)
		{
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000F6EB3 File Offset: 0x000F50B3
		public virtual void Notify_ColorChanged()
		{
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x000F6EB5 File Offset: 0x000F50B5
		public virtual IEnumerable<ThingDefCountClass> GetAdditionalLeavings(Map map, DestroyMode mode)
		{
			return Enumerable.Empty<ThingDefCountClass>();
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x000F6EBC File Offset: 0x000F50BC
		public virtual IEnumerable<ThingDefCountClass> GetAdditionalHarvestYield()
		{
			return Enumerable.Empty<ThingDefCountClass>();
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x000F6EC3 File Offset: 0x000F50C3
		public virtual void CompDrawWornExtras()
		{
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x000F6EC5 File Offset: 0x000F50C5
		public virtual bool CompAllowVerbCast(Verb verb)
		{
			return true;
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x000F6EC8 File Offset: 0x000F50C8
		public virtual bool CompPreventClaimingBy(Faction faction)
		{
			return false;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000F6ECB File Offset: 0x000F50CB
		public virtual bool CompForceDeconstructable()
		{
			return false;
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000F6ECE File Offset: 0x000F50CE
		public virtual float CompGetSpecialApparelScoreOffset()
		{
			return 0f;
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000F6ED5 File Offset: 0x000F50D5
		public virtual void Notify_RecipeProduced(Pawn pawn)
		{
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000F6ED7 File Offset: 0x000F50D7
		public virtual void Notify_DuplicatedFrom(Pawn source)
		{
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000F6ED9 File Offset: 0x000F50D9
		public virtual void Notify_BecameVisible()
		{
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x000F6EDB File Offset: 0x000F50DB
		public virtual void Notify_BecameInvisible()
		{
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x000F6EDD File Offset: 0x000F50DD
		public virtual void Notify_ForcedVisible()
		{
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x000F6EDF File Offset: 0x000F50DF
		public virtual float GetStatFactor(StatDef stat)
		{
			return 1f;
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x000F6EE6 File Offset: 0x000F50E6
		public virtual float GetStatOffset(StatDef stat)
		{
			return 0f;
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x000F6EED File Offset: 0x000F50ED
		public virtual void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
		{
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x000F6EEF File Offset: 0x000F50EF
		public virtual void PreSwapMap()
		{
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000F6EF1 File Offset: 0x000F50F1
		public virtual void PostSwapMap()
		{
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000F6EF4 File Offset: 0x000F50F4
		public virtual Color? ForceColor()
		{
			return null;
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000F6F0A File Offset: 0x000F510A
		public virtual bool DontDrawParent()
		{
			return false;
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000F6F0D File Offset: 0x000F510D
		public virtual bool WantHoldWeapon(Pawn pawn)
		{
			return false;
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000F6F10 File Offset: 0x000F5110
		public virtual AcceptanceReport CanEnterPortal()
		{
			return true;
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000F6F18 File Offset: 0x000F5118
		public virtual List<PawnRenderNode> CompRenderNodes()
		{
			return null;
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000F6F1C File Offset: 0x000F511C
		public override string ToString()
		{
			string[] array = new string[6];
			array[0] = base.GetType().Name;
			array[1] = "(parent=";
			int num = 2;
			ThingWithComps thingWithComps = this.parent;
			array[num] = ((thingWithComps != null) ? thingWithComps.ToString() : null);
			array[3] = " at=";
			int num2 = 4;
			ThingWithComps thingWithComps2 = this.parent;
			array[num2] = ((thingWithComps2 != null) ? thingWithComps2.Position : IntVec3.Invalid).ToString();
			array[5] = ")";
			return string.Concat(array);
		}

		// Token: 0x04001C54 RID: 7252
		public ThingWithComps parent;

		// Token: 0x04001C55 RID: 7253
		public CompProperties props;
	}
}
