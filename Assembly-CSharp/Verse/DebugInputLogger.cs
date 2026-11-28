using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000644 RID: 1604
	public static class DebugInputLogger
	{
		// Token: 0x06003241 RID: 12865 RVA: 0x00140464 File Offset: 0x0013E664
		public static void InputLogOnGUI()
		{
			if (!DebugViewSettings.logInput)
			{
				return;
			}
			if (Event.current.type == EventType.mouseDown || Event.current.type == EventType.mouseUp || Event.current.type == EventType.keyDown || Event.current.type == EventType.keyUp || Event.current.type == EventType.scrollWheel)
			{
				Log.Message("Frame " + Time.frameCount.ToString() + ": " + Event.current.ToStringFull());
			}
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x001404E4 File Offset: 0x0013E6E4
		public static string ToStringFull(this Event ev)
		{
			return string.Concat(new string[]
			{
				"(EVENT\ntype=",
				ev.type.ToString(),
				"\nbutton=",
				ev.button.ToString(),
				"\nkeyCode=",
				ev.keyCode.ToString(),
				"\ndelta=",
				ev.delta.ToString(),
				"\nalt=",
				ev.alt.ToString(),
				"\ncapsLock=",
				ev.capsLock.ToString(),
				"\ncharacter=",
				((ev.character != '\0') ? ev.character : ' ').ToString(),
				"\nclickCount=",
				ev.clickCount.ToString(),
				"\ncommand=",
				ev.command.ToString(),
				"\ncommandName=",
				ev.commandName,
				"\ncontrol=",
				ev.control.ToString(),
				"\nfunctionKey=",
				ev.functionKey.ToString(),
				"\nisKey=",
				ev.isKey.ToString(),
				"\nisMouse=",
				ev.isMouse.ToString(),
				"\nmodifiers=",
				ev.modifiers.ToString(),
				"\nmousePosition=",
				ev.mousePosition.ToString(),
				"\nnumeric=",
				ev.numeric.ToString(),
				"\npressure=",
				ev.pressure.ToString(),
				"\nrawType=",
				ev.rawType.ToString(),
				"\nshift=",
				ev.shift.ToString(),
				"\n)"
			});
		}
	}
}
