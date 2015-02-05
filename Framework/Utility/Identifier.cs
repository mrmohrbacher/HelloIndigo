using System;

namespace Blackriverinc.Framework.Utility
	{
	public static class Identifier
		{
		static int _id = 0;

		// Returns a globally sequential ID
		public static int ID
			{
			get
				{
				return ++_id;
				}
			}
		}
	}
