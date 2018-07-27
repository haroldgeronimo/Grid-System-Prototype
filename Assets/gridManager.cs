using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridManager : MonoBehaviour {
	public Vector3 startPt;
	public Vector3 offset;
	public int gridLength;
	public int gridWidth;
	public int gridHeight;
	public Vector3 cellSize;
	public GameObject cellPrefab;
	public GameObject[,,] grid;
	public Cell[,,] gridCells;
	public List<Objects> gridObjects = new List<Objects>();
	// Use this for initialization
	void Start () {
		InitializeGrid();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
public void DeleteGrid(){
	if(this.transform.childCount > 0){
		foreach(Transform child in this.transform){
			
			//Destroy(child.gameObject);
			DestroyImmediate(child.gameObject);
		}
	}
}
	public void InitializeGrid(){	
		DeleteGrid();
		GameObject[,,] g = new GameObject[gridLength,gridWidth,gridHeight];
		gridCells = new Cell[gridLength,gridWidth,gridHeight];
		for (int i = 0; i < gridLength; i++)
		{
			for (int j = 0; j < gridWidth; j++)
			{	
				for (int k = 0; k < gridHeight; k++)
				{	
				//add to prefab
				if(k==0){
				Vector3 tilePos = new Vector3(-gridLength/2 + 0.5f +i,0,-gridWidth/2 + 0.5f + j);
			//	Vector3 orgPos = new Vector3(startPt.x + ((i+1)*cellSize.x) + (offset.x*i),startPt.y + offset.y ,(startPt.z +((j+1)*cellSize.z))+ (offset.z*j));
				g[i,j,k] = Instantiate(cellPrefab,tilePos,Quaternion.Euler(Vector3.right*90));
				
				g[i,j,k].transform.localScale = cellSize;
				
				g[i,j,k].name = "Cell("+i+","+j+")";
				}else{
					g[i,j,k] = new GameObject("Cell("+i+","+j+","+k+")");
				}
				//parent the object
				g[i,j,k].transform.parent = this.transform;
				
				//create meta data
			
				Cell c = g[i,j,k].AddComponent<Cell>();
				c.size = cellSize;
				c.isOccupied = false;

				if(i==0 && j==0 && k==0) //for testing
				c.isOccupied = true;
                if(i==1 && j==0 && k==0) //for testing
				c.isOccupied = true;
				
				c.row = i;
				c.col = j;
				c.h = k;
				gridCells[i,j,k] = c;
				}
				//initialize objects
				
			
			}
		}

		grid = g;
	}

}


/*

startPt.x + ((i+1)*cellSize.x

 */