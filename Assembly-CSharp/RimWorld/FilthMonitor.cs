using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F18 RID: 7960
	internal static class FilthMonitor
	{
		// Token: 0x0600CAE8 RID: 51944 RVA: 0x004A77AC File Offset: 0x004A59AC
		public static void FilthMonitorTick()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			if (FilthMonitor.lastUpdate + 2500 <= Find.TickManager.TicksAbs)
			{
				int num = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer);
				int num2 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && pawn.RaceProps.Humanlike);
				int num3 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && !pawn.RaceProps.Humanlike);
				Log.Message(string.Format("Filth data, per day:\n  {0} filth spawned per pawn\n  {1} filth human-generated per human\n  {2} filth animal-generated per animal\n  {3} filth accumulated per pawn\n  {4} filth dropped per pawn", new object[]
				{
					(float)FilthMonitor.filthSpawned / (float)num / 2500f * 60000f,
					(float)FilthMonitor.filthHumanGenerated / (float)num2 / 2500f * 60000f,
					(float)FilthMonitor.filthAnimalGenerated / (float)num3 / 2500f * 60000f,
					(float)FilthMonitor.filthAccumulated / (float)num / 2500f * 60000f,
					(float)FilthMonitor.filthDropped / (float)num / 2500f * 60000f
				}));
				FilthMonitor.filthSpawned = 0;
				FilthMonitor.filthAnimalGenerated = 0;
				FilthMonitor.filthHumanGenerated = 0;
				FilthMonitor.filthAccumulated = 0;
				FilthMonitor.filthDropped = 0;
				FilthMonitor.lastUpdate = Find.TickManager.TicksAbs;
			}
		}

		// Token: 0x0600CAE9 RID: 51945 RVA: 0x004A792C File Offset: 0x004A5B2C
		public static void Notify_FilthAccumulated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAccumulated++;
		}

		// Token: 0x0600CAEA RID: 51946 RVA: 0x004A7942 File Offset: 0x004A5B42
		public static void Notify_FilthDropped()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthDropped++;
		}

		// Token: 0x0600CAEB RID: 51947 RVA: 0x004A7958 File Offset: 0x004A5B58
		public static void Notify_FilthAnimalGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAnimalGenerated++;
		}

		// Token: 0x0600CAEC RID: 51948 RVA: 0x004A796E File Offset: 0x004A5B6E
		public static void Notify_FilthHumanGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthHumanGenerated++;
		}

		// Token: 0x0600CAED RID: 51949 RVA: 0x004A7984 File Offset: 0x004A5B84
		public static void Notify_FilthSpawned()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthSpawned++;
		}

		// Token: 0x0400817A RID: 33146
		private static int lastUpdate;

		// Token: 0x0400817B RID: 33147
		private static int filthAccumulated;

		// Token: 0x0400817C RID: 33148
		private static int filthDropped;

		// Token: 0x0400817D RID: 33149
		private static int filthAnimalGenerated;

		// Token: 0x0400817E RID: 33150
		private static int filthHumanGenerated;

		// Token: 0x0400817F RID: 33151
		private static int filthSpawned;

		// Token: 0x04008180 RID: 33152
		private const int SampleDuration = 2500;
	}
}
