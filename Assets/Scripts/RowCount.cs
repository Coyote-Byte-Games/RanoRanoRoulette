using UnityEngine;
using System.Collections;

[System.Serializable]
public class RowCount  {

	private int numOfRows = 7;

		public int Value 
		{ 
			get => numOfRows; 
			set => numOfRows = value;
		}
}
