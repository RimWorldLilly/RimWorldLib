using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

// Token: 0x0200001E RID: 30
public class Command_CallBossgroup : Command
{
	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060000E0 RID: 224 RVA: 0x000087BC File Offset: 0x000069BC
	private IEnumerable<FloatMenuOption> FloatMenuOptions
	{
		get
		{
			Command_CallBossgroup.<get_FloatMenuOptions>d__3 <get_FloatMenuOptions>d__ = new Command_CallBossgroup.<get_FloatMenuOptions>d__3(-2);
			<get_FloatMenuOptions>d__.<>4__this = this;
			return <get_FloatMenuOptions>d__;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060000E1 RID: 225 RVA: 0x000087CC File Offset: 0x000069CC
	public override string DescPostfix
	{
		get
		{
			string text = "";
			Dictionary<BossgroupDef, AcceptanceReport> source = DefDatabase<BossgroupDef>.AllDefs.ToDictionary((BossgroupDef b) => b, (BossgroupDef b) => CallBossgroupUtility.BossgroupEverCallable(this.mechanitor.Pawn, b, true));
			foreach (KeyValuePair<BossgroupDef, AcceptanceReport> keyValuePair in from b in source
			where b.Value
			select b)
			{
				text = text + "\n\n" + "ReadyToSummonThreat".Translate(keyValuePair.Key.boss.kindDef.label).Colorize(ColorLibrary.Green).CapitalizeFirst();
			}
			foreach (KeyValuePair<BossgroupDef, AcceptanceReport> keyValuePair2 in from b in source
			where !b.Value
			select b)
			{
				text = text + "\n\n" + ("CannotSummon".Translate(keyValuePair2.Key.boss.kindDef.label) + ": " + keyValuePair2.Value.Reason).Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00008960 File Offset: 0x00006B60
	public Command_CallBossgroup(Pawn_MechanitorTracker mechanitor)
	{
		this.mechanitor = mechanitor;
		this.defaultLabel = "CommandCallBossgroup".Translate();
		this.defaultDesc = "CommandCallBossgroupDesc".Translate();
		this.icon = Command_CallBossgroup.BossgroupTex.Texture;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000089B4 File Offset: 0x00006BB4
	public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
	{
		this.disabled = this.IsDisabled(out this.disabledReason);
		return base.GizmoOnGUI(topLeft, maxWidth, parms);
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000089D4 File Offset: 0x00006BD4
	private bool IsDisabled(out string reason)
	{
		int lastBossgroupCalled = Find.BossgroupManager.lastBossgroupCalled;
		int num = Find.TickManager.TicksGame - lastBossgroupCalled;
		if (Faction.OfMechanoids == null || Faction.OfMechanoids.deactivated)
		{
			reason = "MechsDisabled".Translate();
			return true;
		}
		if (!this.mechanitor.Pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			reason = "Incapable".Translate().CapitalizeFirst();
			return true;
		}
		if (num < 120000)
		{
			reason = "BossgroupAvailableIn".Translate((120000 - num).ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst();
			return true;
		}
		PawnKindDef pendingBossgroup = CallBossgroupUtility.GetPendingBossgroup();
		if (pendingBossgroup != null)
		{
			reason = "BossgroupIncoming".Translate(pendingBossgroup.label).CapitalizeFirst();
			return true;
		}
		if (this.FloatMenuOptions.All((FloatMenuOption f) => f.action == null))
		{
			reason = null;
			return true;
		}
		reason = null;
		return false;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00008AF8 File Offset: 0x00006CF8
	public override void ProcessInput(Event ev)
	{
		base.ProcessInput(ev);
		List<FloatMenuOption> list = new List<FloatMenuOption>();
		list.AddRange(this.FloatMenuOptions);
		Find.WindowStack.Add(new FloatMenu(list));
	}

	// Token: 0x04000044 RID: 68
	private static readonly CachedTexture BossgroupTex = new CachedTexture("UI/Icons/SummonMechThreat");

	// Token: 0x04000045 RID: 69
	private Pawn_MechanitorTracker mechanitor;
}
