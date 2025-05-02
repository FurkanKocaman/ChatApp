// modal.service.ts
import {
  Injectable,
  TemplateRef,
  ViewContainerRef,
  ComponentFactoryResolver,
  Injector,
  ComponentRef,
  Type,
  ComponentFactory,
  ApplicationRef,
  createComponent,
  EnvironmentInjector,
} from "@angular/core";
import { ModalComponent } from "../../shared/components/modal/modal.component";

@Injectable({ providedIn: "root" })
export class ModalService {
  private modalComponentRef: ComponentRef<ModalComponent> | null = null;

  constructor(private environmentInjector: EnvironmentInjector, private appRef: ApplicationRef) {}

  open(component: Type<any>, inputs?: Record<string, any>) {
    this.close(); // Var olan modal varsa kapat

    // Modal bileşenini oluştur
    this.modalComponentRef = createComponent(ModalComponent, {
      environmentInjector: this.appRef.injector,
    });

    // İçerik bileşenini oluştur
    const contentRef = createComponent(component, {
      environmentInjector: this.environmentInjector,
    });

    // Inputları ayarla
    if (inputs) {
      Object.entries(inputs).forEach(([key, value]) => {
        (contentRef.instance as any)[key] = value;
      });
    }

    // İçeriği modal içerisine yerleştir
    this.modalComponentRef.instance.modalContent.insert(contentRef.hostView);

    // DOM'a ekle
    this.appRef.attachView(this.modalComponentRef.hostView);
    document.body.appendChild(this.modalComponentRef.location.nativeElement);

    // Kapatma event'ini dinle
    this.modalComponentRef.instance.closeEvent.subscribe(() => this.close());
  }

  close() {
    if (this.modalComponentRef) {
      this.appRef.detachView(this.modalComponentRef.hostView);
      this.modalComponentRef.destroy();
      this.modalComponentRef = null;
    }
  }
}
