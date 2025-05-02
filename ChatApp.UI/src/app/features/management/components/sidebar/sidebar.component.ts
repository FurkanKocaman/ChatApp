import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Output } from "@angular/core";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";

@Component({
  selector: "app-sidebar",
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: "./sidebar.component.html",
  styleUrl: "./sidebar.component.css",
})
export class SidebarComponent {
  @Output() closeSidebar = new EventEmitter<void>();

  constructor(private router: Router) {}

  close() {
    if (window.innerWidth <= 768) {
      this.closeSidebar.emit();
    }
  }

  isRouteIncludes(text: string): boolean {
    return this.router.url.includes(text);
  }
}
