import { Component, ElementRef, OnInit, ViewChild, Input } from '@angular/core';
import * as PIXI from 'pixi.js';
import { gsap } from 'gsap';
import { MotionPathPlugin } from 'gsap/MotionPathPlugin';

@Component({
  selector: 'app-visualization',
  templateUrl: './visualization.component.html',
  styleUrls: ['./visualization.component.scss'],
})
export class VisualizationComponent implements OnInit {
  @ViewChild('pixiCanvas', { static: true })
  pixiCanvas!: ElementRef<HTMLCanvasElement>;
  @Input() type: string = '';
  sprite1: any;
  sprite2: any;
  cash: any;
  app: PIXI.Application;

  ngOnInit(): void {
    const canvas = this.pixiCanvas.nativeElement;
    const parent = canvas.parentElement as HTMLElement;
    this.app = new PIXI.Application({
      view: this.pixiCanvas.nativeElement,
      width: window.innerWidth / 2,
      height: window.innerHeight / 2,
      backgroundColor: 0xffffff,
    });

    switch (this.type) {
      case 'deposit': {
        this.sprite1 = this.createUser();
        this.sprite2 = this.createBank();
        break;
      }
      case 'withdrawal': {
        this.sprite1 = this.createBank();
        this.sprite2 = this.createUser();
        break;
      }
      default: {
        this.sprite1 = this.createUser();
        this.sprite2 = this.createUser();
      }
    }

    this.cash = this.createCash();
    const height = this.app.renderer.height * 0.9;
    const width = this.app.renderer.width * 0.9;

    this.sprite1.x = 20;
    this.sprite1.y = 20;
    this.sprite2.x = this.app.renderer.width - 200;
    this.sprite2.y = this.app.renderer.height - 160;

    this.cash.x = width * 0.05;
    this.cash.y = height * 0.05;
    this.cash.width = 100;
    this.cash.height = 100;

    const customPath = [
      { x: width * 0.3, y: height * 0.2 },
      { x: width * 0.2, y: height * 0.5 },
      { x: width * 0.5, y: height * 0.35 },
      { x: width * 0.8, y: height * 0.6 },
      { x: width * 0.6, y: height * 0.8 },
      { x: width * 0.95, y: height * 0.85 },
    ];

    this.app.stage.addChild(this.sprite1);
    this.app.stage.addChild(this.sprite2);
    this.app.stage.addChild(this.cash);

    this.animate(customPath);
  }

  createUser() {
    const texture = PIXI.Texture.from('../../assets/images/user.png');
    const newSprite = new PIXI.Sprite(texture);
    newSprite.width = 200;
    newSprite.height = 160;
    return newSprite;
  }

  createBank() {
    const texture = PIXI.Texture.from('../../assets/images/bank.png');
    const newSprite = new PIXI.Sprite(texture);
    newSprite.width = 150;
    newSprite.height = 130;
    return newSprite;
  }

  createCash() {
    const texture = PIXI.Texture.from('../../assets/images/cash.png');
    return new PIXI.Sprite(texture);
  }

  animate(path: { x: number; y: number }[]) {
    gsap.registerPlugin(MotionPathPlugin);
    gsap.to(this.cash, {
      duration: 2.5,
      ease: 'linear',
      motionPath: {
        path: path,
        align: 'self',
      },
      repeat: -1,
      yoyo: false,
    });
  }
}
