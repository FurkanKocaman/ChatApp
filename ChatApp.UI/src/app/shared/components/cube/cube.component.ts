import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  OnInit,
  ViewChild,
} from "@angular/core";
import { OrbitControls } from "three/examples/jsm/controls/OrbitControls.js";
import * as THREE from "three";

@Component({
  selector: "app-cube",
  standalone: true,
  imports: [],
  templateUrl: "./cube.component.html",
  styleUrl: "./cube.component.css",
})
export class CubeComponent implements OnInit, AfterViewInit {
  @ViewChild("canvas") canvasRef!: ElementRef<HTMLCanvasElement>;

  scene!: THREE.Scene;
  camera!: THREE.PerspectiveCamera;
  renderer!: THREE.WebGLRenderer;
  cubes: THREE.Mesh[] = [];

  targetPositions: THREE.Vector3[] = [];
  isGathering: boolean = true; // true = merkeze toplanıyor, false = dağılacak
  timer: number = 0;

  ngOnInit(): void {
    const container = document.querySelector(".relative.w-full.h-full");

    // Get its dimensions
    const width = container!.clientWidth;
    const height = container!.clientHeight;

    // Set the renderer size based on the container
    this.renderer.setSize(width, height);
  }

  ngAfterViewInit(): void {
    this.initThree();
    this.animate();
  }

  initThree() {
    this.scene = new THREE.Scene();
    this.scene.background = new THREE.Color(0x1a1a1a);

    this.camera = new THREE.PerspectiveCamera(
      75,
      window.innerWidth / window.innerHeight,
      0.1,
      1000
    );
    this.camera.position.z = 20;

    this.renderer = new THREE.WebGLRenderer({
      canvas: this.canvasRef.nativeElement,
      antialias: true,
    });
    // this.renderer.setSize(200, 100);

    const geometry = new THREE.BoxGeometry(1, 1, 1);
    const material = new THREE.MeshStandardMaterial({ color: 0x00ffff });

    for (let i = 0; i < 100; i++) {
      const cube = new THREE.Mesh(geometry, material);
      cube.position.set(
        (Math.random() - 0.5) * 20,
        (Math.random() - 0.5) * 20,
        (Math.random() - 0.5) * 20
      );
      this.scene.add(cube);
      this.cubes.push(cube);
      this.targetPositions.push(new THREE.Vector3(0, 0, 0)); // ilk hedef = merkez
    }

    const light = new THREE.PointLight(0xffffff, 1);
    light.position.set(10, 10, 10);
    this.scene.add(light);
  }

  animate = () => {
    requestAnimationFrame(this.animate);

    // Zamanı ilerlet
    this.timer += 1;

    // Eğer 200 frame boyunca toplandıysa, dağılmaya başlasın
    if (this.timer > 200 && this.isGathering) {
      this.isGathering = false;
      this.timer = 0;

      // Yeni rastgele hedefler belirle
      this.targetPositions.forEach((target) => {
        target.set(
          (Math.random() - 0.5) * 20,
          (Math.random() - 0.5) * 20,
          (Math.random() - 0.5) * 20
        );
      });
    }

    // Eğer tekrar 400 frame geçerse tekrar toplanmaya başlasın
    if (this.timer > 400 && !this.isGathering) {
      this.isGathering = true;
      this.timer = 0;

      // Hedef tekrar merkez olsun
      this.targetPositions.forEach((target) => {
        target.set(0, 0, 0);
      });
    }

    // Küpler hedeflerine doğru yavaşça ilerlesin
    this.cubes.forEach((cube, i) => {
      cube.position.lerp(this.targetPositions[i], 0.05); // 0.05 oranında yaklaş
    });

    this.renderer.render(this.scene, this.camera);
  };
  @HostListener("window:resize", ["$event"])
  onResize() {
    const container = document.querySelector(".relative.w-full.h-full");

    // Get updated container size
    const width = container!.clientWidth;
    const height = container!.clientHeight;

    // Update renderer size
    this.renderer.setSize(width, height);
  }
}
