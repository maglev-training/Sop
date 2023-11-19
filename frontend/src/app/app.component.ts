import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { userFeature } from './state/user';
import { UserCommands } from './state/user/actions';

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
  isAuthenticated = this.store.selectSignal(userFeature.selectIsAuthenticated);

  ngOnInit(): void {
      if(this.isAuthenticated() === false) {
        this.store.dispatch(UserCommands.checkAuth());
      }
  }
  logOut() {
      this.store.dispatch(UserCommands.logOut());
  }
}
