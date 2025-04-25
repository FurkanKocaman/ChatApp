import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-main-layout",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./main-layout.component.html",
  styleUrl: "./main-layout.component.css",
})
export class MainLayoutComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
