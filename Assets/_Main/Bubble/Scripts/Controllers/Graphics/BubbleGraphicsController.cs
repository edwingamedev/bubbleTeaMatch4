﻿using UnityEngine;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleGraphicsController : MonoBehaviour, IGraphicsController
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BubbleSettings bubbleSettings;

        public void DisableHighlight()
        {
            spriteRenderer.material.SetFloat("_HighLight", 0);
        }

        public void EnableHighlight()
        {
            spriteRenderer.material.SetFloat("_HighLight", 1);
        }

        public void SetMaterial(Material material)
        {
            spriteRenderer.material = material;
        }

        public void PopAnimation()
        {
            animator.enabled = true;
            animator.SetBool("Pop", true);
        }

        public void ResetAnimation()
        {
            animator.SetBool("Pop", false);
            animator.enabled = false;
        }

        public void UpdateGraphics(ConnectionOrientation connection)
        {
            ResetAnimation();
            spriteRenderer.sprite = bubbleSettings.SpriteConnections.GetSprite(connection);            
        }
    }
}