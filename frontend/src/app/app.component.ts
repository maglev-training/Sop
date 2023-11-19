import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { authFeature } from './state/auth';
import { AuthCommands } from './state/auth/actions';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'frontend';
  private readonly store = inject(Store);
  isAuthenticated = this.store.selectSignal(authFeature.selectIsAuthenticated);

  ngOnInit(): void {
      if(this.isAuthenticated() === false) {
        this.store.dispatch(AuthCommands.checkAuth());
      }
  }
  logOut() {
      this.store.dispatch(AuthCommands.logOut());
  }
}
