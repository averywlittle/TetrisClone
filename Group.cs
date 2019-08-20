using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{

    float lastFall = 0;

    // Verifies each child block's position
    bool isValidGridPos() {
        foreach (Transform child in transform) {
            Vector2 v = Playfield.roundVec2(child.position);

            // Not inside border?
            if (!Playfield.insideBorder(v))
                return false;

            // Block in grid cell (and not part of the same group)?
            if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                Playfield.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    // If a group changed its position, then it has to remove all the old block positions from the grid and add all the new block positions to the grid
    void updateGrid() {
        // Remove old children from grid
        for (int y = 0; y < Playfield.h; ++y)
            for (int x = 0; x < Playfield.w; ++x)
                if (Playfield.grid[x, y] != null)
                    if (Playfield.grid[x, y].parent == transform)
                        Playfield.grid[x, y] = null;

        // Add new children to the grid
        foreach (Transform child in transform) {
            Vector2 v = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }

    // Start is called before the first frame update
    void Start() {
        // Default position not valid? Then it's game over
        if (!isValidGridPos()) {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            // See if it's valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. Revert.
                transform.position += new Vector3(1, 0, 0);
        }

        // Move right
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            // Modify position
            transform.position += new Vector3(1, 0, 0);

            // See if it's valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. Revert.
                transform.position += new Vector3(-1, 0, 0);
        }

        // Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            // Modify position
            transform.Rotate(0, 0, -90);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. Revert.
                transform.position += new Vector3(0, 0, 90);
        }

        // Fall and move downwards
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1) {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else {
                // It's not valid. Revert.
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                Playfield.deleteFullRows();

                // Spawn next group
                FindObjectOfType<Spawner>().spawnNext();

                // Disable script
                enabled = false;
            }

            lastFall= Time.time;
        }
    }
}
