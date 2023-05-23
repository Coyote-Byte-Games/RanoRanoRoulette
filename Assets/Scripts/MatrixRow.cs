using UnityEngine;
using System.Collections;

[System.Serializable]
public class MatrixRow  {
		public Sprite[] row;
		public Sprite[] Value 
		{ 
			get => row; 
			set => row = value; 
		}
}