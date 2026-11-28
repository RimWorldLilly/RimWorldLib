using System;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A7 RID: 4263
	public class QuestPart_AddGiverQuest : QuestPart_AddQuest
	{
		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06006AB0 RID: 27312 RVA: 0x00257C69 File Offset: 0x00255E69
		public override QuestScriptDef QuestDef
		{
			get
			{
				return this.questScript;
			}
		}

		// Token: 0x06006AB1 RID: 27313 RVA: 0x00257C74 File Offset: 0x00255E74
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			NamedArgument[] args = (from arg in this.receivedSignal.args.Args
			select arg.arg.Named(arg.label)).ToArray<NamedArgument>();
			slate.Set<float>("points", this.points, false);
			slate.Set<TaggedString>("discoveryMethod", this.discoveryMethodTranslationKey.Translate(args), false);
			return slate;
		}

		// Token: 0x06006AB2 RID: 27314 RVA: 0x00257CEA File Offset: 0x00255EEA
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.receivedSignal = signal;
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06006AB3 RID: 27315 RVA: 0x00257D02 File Offset: 0x00255F02
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<QuestScriptDef>(ref this.questScript, "questScript");
			Scribe_Values.Look<string>(ref this.discoveryMethodTranslationKey, "discoveryMethodTranslationKey", null, false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
		}

		// Token: 0x040043CD RID: 17357
		public QuestScriptDef questScript;

		// Token: 0x040043CE RID: 17358
		public string discoveryMethodTranslationKey;

		// Token: 0x040043CF RID: 17359
		public float points;

		// Token: 0x040043D0 RID: 17360
		private Signal receivedSignal;
	}
}
