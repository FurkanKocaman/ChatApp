import { Component, inject, Input } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { Channel } from "../../../../../core/models/entities";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-channel-modal",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: "./channel-modal.component.html",
  styleUrl: "./channel-modal.component.css",
})
export class ChannelModalComponent {
  @Input() channel?: Channel;

  private fb = inject(FormBuilder);

  form = this.fb.group({
    name: ["", Validators.required],
    description: [""],
    iconUrl: [""],
    isPublic: [true, Validators.required],
    type: [0, Validators.required],
    serverId: ["", Validators.required],
  });

  onSubmit() {
    console.log(this.form.value);
  }
}
