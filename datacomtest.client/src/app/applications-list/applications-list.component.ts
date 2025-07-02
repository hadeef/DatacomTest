import { Component, OnInit } from '@angular/core';
import { ApplicationService, Application } from '../application.service';

@Component({
  selector: 'app-applications-list',
  templateUrl: './applications-list.component.html',
  styleUrls: ['./applications-list.component.css']
})
export class ApplicationsListComponent implements OnInit {
  applications: Application[] = [];
  loading = true;
  error = '';
  showForm = false;
  formModel: Partial<Application> = {};
  pageNumber = 1;
  pageSize = 5;

  constructor(private applicationService: ApplicationService) { }

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications() {
    this.loading = true;
    this.applicationService.getAllApplications().subscribe({
      next: (data) => {
        this.applications = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load applications';
        this.loading = false;
      }
    });
  }

  openAddForm() {
    this.error = '';
    this.formModel = { status: 0, dateApplied: new Date().toISOString().substring(0, 10) };
    this.showForm = true;
  }

  editApplication(app: Application) {
    this.error = '';
    this.formModel = {
      ...app,
      dateApplied: app.dateApplied ? app.dateApplied.substring(0, 10) : '' // Format date to YYYY-MM-DD to fill date input correctly.
    };
    this.showForm = true;
  }

  closeForm() {
    this.error = '';
    this.showForm = false;
    this.formModel = {};
  }

  onSubmit() {
    if (this.formModel.id) {
      // Update
      this.applicationService.updateApplication(this.formModel as Application).subscribe({
        next: () => {
          this.loadApplications();
          this.closeForm();
          this.error = '';
        },
        error: (err) => {
          this.error = this.extractErrorMessage(err);
        }
      });
    } else {
      // Add
      this.applicationService.addApplication(this.formModel as Application).subscribe({
        next: () => {
          this.loadApplications();
          this.closeForm();
          this.error = '';
        },
        error: (err) => {
          this.error = this.extractErrorMessage(err);
        }
      });
    }
  }

  updateStatus(app: Application) {
    this.applicationService.updateApplication(app).subscribe({
      next: () => {
        this.loadApplications();
        this.closeForm();
        this.error = '';
      },
      error: (err) => {
        this.error = this.extractErrorMessage(err);
      }
    });
  }

  // Helper function to extract error message from HTTP response
  extractErrorMessage(err: any): string {
    if (err.error && typeof err.error === 'string') {
      return err.error;
    }
    if (err.error && err.error.message) {
      return err.error.message;
    }
    if (err.message) {
      return err.message;
    }
    return 'An unexpected error occurred.';
  }

  // Pagination
  pagedApplications(): Application[] {
    const start = (this.pageNumber - 1) * this.pageSize;
    return this.applications.slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.applications.length / this.pageSize);
  }
  nextPage() {
    if (this.pageNumber < this.totalPages) this.pageNumber++;
  }
  prevPage() {
    if (this.pageNumber > 1) this.pageNumber--;
  }
}
