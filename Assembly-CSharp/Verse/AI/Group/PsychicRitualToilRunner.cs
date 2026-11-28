using System;
using System.Runtime.CompilerServices;

namespace Verse.AI.Group
{
	// Token: 0x0200091B RID: 2331
	public static class PsychicRitualToilRunner
	{
		// Token: 0x0600470F RID: 18191 RVA: 0x001BE618 File Offset: 0x001BC818
		public static void Start(PsychicRitualToil toil, PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			try
			{
				if (toil != null)
				{
					toil.Start(psychicRitual, parent);
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 2);
				defaultInterpolatedStringHandler.AppendLiteral("PsychicRitualToil ");
				defaultInterpolatedStringHandler.AppendFormatted(toil.ToStringSafe<PsychicRitualToil>());
				defaultInterpolatedStringHandler.AppendLiteral(" threw an exception during Start(): ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x001BE688 File Offset: 0x001BC888
		public static bool Tick(PsychicRitualToil toil, PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			try
			{
				return toil == null || toil.Tick(psychicRitual, parent);
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
				defaultInterpolatedStringHandler.AppendLiteral("PsychicRitualToil ");
				defaultInterpolatedStringHandler.AppendFormatted(toil.ToStringSafe<PsychicRitualToil>());
				defaultInterpolatedStringHandler.AppendLiteral(" threw an exception during Tick(): ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return true;
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x001BE700 File Offset: 0x001BC900
		public static void UpdateAllDuties(PsychicRitualToil toil, PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			try
			{
				if (toil != null)
				{
					toil.UpdateAllDuties(psychicRitual, parent);
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 2);
				defaultInterpolatedStringHandler.AppendLiteral("PsychicRitualToil ");
				defaultInterpolatedStringHandler.AppendFormatted(toil.ToStringSafe<PsychicRitualToil>());
				defaultInterpolatedStringHandler.AppendLiteral(" threw an exception during UpdateAllDuties(): ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x001BE770 File Offset: 0x001BC970
		public static void End(PsychicRitualToil toil, PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
			try
			{
				if (toil != null)
				{
					toil.End(psychicRitual, parent, success);
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
				defaultInterpolatedStringHandler.AppendLiteral("PsychicRitualToil ");
				defaultInterpolatedStringHandler.AppendFormatted(toil.ToStringSafe<PsychicRitualToil>());
				defaultInterpolatedStringHandler.AppendLiteral(" threw an exception during End(): ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}
	}
}
