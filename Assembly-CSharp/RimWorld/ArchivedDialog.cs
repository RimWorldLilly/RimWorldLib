using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102A RID: 4138
	public class ArchivedDialog : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x0600683E RID: 26686 RVA: 0x0024DCC1 File Offset: 0x0024BEC1
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x0600683F RID: 26687 RVA: 0x0024DCC4 File Offset: 0x0024BEC4
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x06006840 RID: 26688 RVA: 0x0024DCCB File Offset: 0x0024BECB
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x06006841 RID: 26689 RVA: 0x0024DCD8 File Offset: 0x0024BED8
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x06006842 RID: 26690 RVA: 0x0024DCE0 File Offset: 0x0024BEE0
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x06006843 RID: 26691 RVA: 0x0024DCE8 File Offset: 0x0024BEE8
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x06006844 RID: 26692 RVA: 0x0024DCEB File Offset: 0x0024BEEB
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06006845 RID: 26693 RVA: 0x0024DCEE File Offset: 0x0024BEEE
		public ArchivedDialog()
		{
		}

		// Token: 0x06006846 RID: 26694 RVA: 0x0024DCF8 File Offset: 0x0024BEF8
		public ArchivedDialog(string text, string title = null, Faction relatedFaction = null)
		{
			this.text = text;
			this.title = title;
			this.relatedFaction = relatedFaction;
			this.createdTick = GenTicks.TicksGame;
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextArchivedDialogID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x06006847 RID: 26695 RVA: 0x0024DD50 File Offset: 0x0024BF50
		void IArchivable.OpenArchived()
		{
			DiaNode diaNode = new DiaNode(this.text);
			DiaOption diaOption = new DiaOption("OK".Translate());
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, false, this.title));
		}

		// Token: 0x06006848 RID: 26696 RVA: 0x0024DDB4 File Offset: 0x0024BFB4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.createdTick, "createdTick", 0, false);
		}

		// Token: 0x06006849 RID: 26697 RVA: 0x0024DE1A File Offset: 0x0024C01A
		public string GetUniqueLoadID()
		{
			return "ArchivedDialog_" + this.ID.ToString();
		}

		// Token: 0x04004262 RID: 16994
		public int ID;

		// Token: 0x04004263 RID: 16995
		public string text;

		// Token: 0x04004264 RID: 16996
		public string title;

		// Token: 0x04004265 RID: 16997
		public Faction relatedFaction;

		// Token: 0x04004266 RID: 16998
		public int createdTick;
	}
}
