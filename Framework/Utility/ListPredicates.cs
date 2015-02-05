using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Blackriverinc.DataAccess
	{
	public static class ListPredicates
		{
		public static IEnumerable<ItemType> RandomSubset<ItemType>(this IEnumerable<ItemType> sourceList,
													 Func<ItemType, object> keySelector,
													 int sampleCount)
			{
			List<ItemType> result = new List<ItemType>();

			try
				{
				//------------------------------------------------------------
				// Return a new List from a randomly selected sub-set of
				// an initial List.
				Func<IEnumerable<ItemType>, int, IEnumerable<ItemType>> randomList
					= ((IEnumerable<ItemType> list, int limit) =>
					{
						List<ItemType> randomResults = new List<ItemType>();
						Random rg = new Random(System.DateTime.Now.Millisecond);

						int listCount = list.Count<ItemType>();
						if (limit > listCount)
							limit = listCount;
						for (int i = 0; i < limit; )
							{
							int hitIndex = (listCount > limit) ? rg.Next(listCount) : i;
							ItemType cat = list.ElementAt(hitIndex);

							if (!randomResults.Contains(cat, keySelector))
								{
								randomResults.Add(cat);
								i++;
								}
							}
						return randomResults;
					});

				//------------------------------------------------------------
				// Verify that the keySelector covers the input set uniquely;
				// if it does not, the "random" selection may never end.
				//------------------------------------------------------------
				// TODO

				//------------------------------------------------------------
				// Generate a List of Random Items from the Source set.
				//------------------------------------------------------------
				var sampleList = from X in
										  randomList((from item
															 in sourceList
														  select item), sampleCount)
									  select X;
				result.AddRange(sampleList);

				}
			catch (Exception exp)
				{
				Trace.WriteLine(exp.ToString());
				throw;
				}

			return result;
			}

		}
	}
