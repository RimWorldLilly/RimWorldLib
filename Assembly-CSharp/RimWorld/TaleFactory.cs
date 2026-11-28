using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020017D5 RID: 6101
	public static class TaleFactory
	{
		// Token: 0x06009838 RID: 38968 RVA: 0x00372F80 File Offset: 0x00371180
		public static Tale MakeRawTale(TaleDef def, params object[] args)
		{
			Tale result;
			try
			{
				Tale tale = (Tale)Activator.CreateInstance(def.taleClass, args);
				tale.def = def;
				tale.id = Find.UniqueIDsManager.GetNextTaleID();
				tale.date = Find.TickManager.TicksAbs;
				result = tale;
			}
			catch (Exception arg)
			{
				Exception arg2;
				Log.Error(string.Format("Failed to create tale object {0} with parameters {1}: {2}", def, (from arg in args
				select arg.ToStringSafe<object>()).ToCommaList(false, false), arg2));
				result = null;
			}
			return result;
		}

		// Token: 0x06009839 RID: 38969 RVA: 0x0037301C File Offset: 0x0037121C
		public static Tale MakeRandomTestTale(TaleDef def = null)
		{
			if (def == null)
			{
				def = (from d in DefDatabase<TaleDef>.AllDefs
				where d.usableForArt
				select d).RandomElement<TaleDef>();
			}
			Tale tale = TaleFactory.MakeRawTale(def, Array.Empty<object>());
			tale.GenerateTestData();
			return tale;
		}
	}
}
