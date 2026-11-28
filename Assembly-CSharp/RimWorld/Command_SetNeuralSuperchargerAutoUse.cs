using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBD RID: 3773
	[StaticConstructorOnStartup]
	public class Command_SetNeuralSuperchargerAutoUse : Command
	{
		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06006352 RID: 25426 RVA: 0x0023B043 File Offset: 0x00239243
		private static Texture2D AutoUseForEveryone
		{
			get
			{
				if (Command_SetNeuralSuperchargerAutoUse.autoUseForEveryone == null)
				{
					Command_SetNeuralSuperchargerAutoUse.autoUseForEveryone = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_EveryoneAutoUse", true);
				}
				return Command_SetNeuralSuperchargerAutoUse.autoUseForEveryone;
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06006353 RID: 25427 RVA: 0x0023B067 File Offset: 0x00239267
		private static Texture2D AutoUseWithDesire
		{
			get
			{
				if (Command_SetNeuralSuperchargerAutoUse.autoUseWithDesire == null)
				{
					Command_SetNeuralSuperchargerAutoUse.autoUseWithDesire = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_AutoUseWithDesire", true);
				}
				return Command_SetNeuralSuperchargerAutoUse.autoUseWithDesire;
			}
		}

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06006354 RID: 25428 RVA: 0x0023B08B File Offset: 0x0023928B
		private static Texture2D NoAutoUseTex
		{
			get
			{
				if (Command_SetNeuralSuperchargerAutoUse.noAutoUseTex == null)
				{
					Command_SetNeuralSuperchargerAutoUse.noAutoUseTex = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_NoAutoUse", true);
				}
				return Command_SetNeuralSuperchargerAutoUse.noAutoUseTex;
			}
		}

		// Token: 0x06006355 RID: 25429 RVA: 0x0023B0B0 File Offset: 0x002392B0
		public Command_SetNeuralSuperchargerAutoUse(CompNeuralSupercharger comp)
		{
			this.comp = comp;
			switch (comp.autoUseMode)
			{
			case CompNeuralSupercharger.AutoUseMode.NoAutoUse:
				this.defaultLabel = "CommandNeuralSuperchargerNoAutoUse".Translate();
				this.defaultDesc = "CommandNeuralSuperchargerNoAutoUseDescription".Translate();
				this.icon = Command_SetNeuralSuperchargerAutoUse.NoAutoUseTex;
				return;
			case CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire:
				this.defaultLabel = "CommandNeuralSuperchargerAutoUseWithDesire".Translate();
				this.defaultDesc = "CommandNeuralSuperchargerAutoUseWithDesireDescription".Translate();
				this.icon = Command_SetNeuralSuperchargerAutoUse.AutoUseWithDesire;
				return;
			case CompNeuralSupercharger.AutoUseMode.AutoUseForEveryone:
				this.defaultLabel = "CommandNeuralSuperchargerAutoForEveryone".Translate();
				this.defaultDesc = "CommandNeuralSuperchargerAutoForEveryoneDescription".Translate();
				this.icon = Command_SetNeuralSuperchargerAutoUse.AutoUseForEveryone;
				return;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown auto use mode: ");
				defaultInterpolatedStringHandler.AppendFormatted<CompNeuralSupercharger.AutoUseMode>(comp.autoUseMode);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			}
		}

		// Token: 0x06006356 RID: 25430 RVA: 0x0023B1BC File Offset: 0x002393BC
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("CommandNeuralSuperchargerNoAutoUse".Translate(), delegate()
			{
				this.comp.autoUseMode = CompNeuralSupercharger.AutoUseMode.NoAutoUse;
			}, Command_SetNeuralSuperchargerAutoUse.NoAutoUseTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandNeuralSuperchargerAutoUseWithDesire".Translate(), delegate()
			{
				this.comp.autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire;
			}, Command_SetNeuralSuperchargerAutoUse.AutoUseWithDesire, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandNeuralSuperchargerAutoForEveryone".Translate(), delegate()
			{
				this.comp.autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseForEveryone;
			}, Command_SetNeuralSuperchargerAutoUse.AutoUseForEveryone, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0400377E RID: 14206
		private static Texture2D autoUseForEveryone;

		// Token: 0x0400377F RID: 14207
		private static Texture2D autoUseWithDesire;

		// Token: 0x04003780 RID: 14208
		private static Texture2D noAutoUseTex;

		// Token: 0x04003781 RID: 14209
		private readonly CompNeuralSupercharger comp;
	}
}
